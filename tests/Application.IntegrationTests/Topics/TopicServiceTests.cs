using AutoFixture;
using FluentAssertions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Requests;
using GroupProject.Application.IntegrationTests.Common.Fixtures;
using GroupProject.Application.Topics;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.IntegrationTests.Topics;

public class TopicServiceTests
{
    private readonly DatabaseFixture _db;
    private readonly IAppDbContext _dbContext;
    private readonly ITopicService _topics;

    public TopicServiceTests(
        DatabaseFixture db,
        IAppDbContext dbContext,
        ITopicService topics)
    {
        _db = db;
        _dbContext = dbContext;
        _topics = topics;
    }

    [Fact]
    public async Task Get_ByUserId()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
        {
            var user = random.NextDouble() >= 0.5 ? await _db.CreateUserAsync() : _db.DefaultUser;
            await _db.CreateTopicAsync(user.Id, _db.DefaultSection.Id);
        }

        var request = new GetTopicsRequest(
            new PageRequest(1, 10),
            TopicsOrderedBy.CreationTime,
            UserId: _db.DefaultUser.Id);

        var response = await _topics.Get(request, CancellationToken.None);

        response.List.Should().OnlyContain(t => t.SectionId == _db.DefaultSection.Id);
    }

    [Fact]
    public async Task Get_BySectionId()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
        {
            var section = random.NextDouble() >= 0.5 ? await _db.CreateSectionAsync() : _db.DefaultSection;
            await _db.CreateTopicAsync(_db.DefaultUser.Id, section.Id);
        }

        var request = new GetTopicsRequest(
            new PageRequest(1, 10),
            TopicsOrderedBy.CreationTime,
            SectionId: _db.DefaultSection.Id);

        var response = await _topics.Get(request, CancellationToken.None);

        response.List.Should().OnlyContain(t => t.SectionId == _db.DefaultSection.Id);
    }

    [Fact]
    public async Task Get_OnlyOpen()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
        {
            var topic = await _db.CreateTopicAsync();
            _dbContext.Set<Topic>().Attach(topic);
            if (random.NextDouble() >= 0.5) topic.SetClosed();
        }

        await _dbContext.SaveChangesAsync();

        var request = new GetTopicsRequest(new PageRequest(1, 10), TopicsOrderedBy.CreationTime, true);
        var response = await _topics.Get(request, CancellationToken.None);

        response.List.Should().OnlyContain(t => !t.IsClosed);
    }

    [Fact]
    public async Task Get_ContainsSubstring()
    {
        for (var i = 0; i < 10; i++) await _db.CreateTopicAsync();

        var request = new GetTopicsRequest(new PageRequest(1, 10), TopicsOrderedBy.CreationTime, Substring: "a");
        var response = await _topics.Get(request, CancellationToken.None);

        response.List.Should().OnlyContain(t => t.Header.Contains(request.Substring!));
    }

    [Fact]
    public async Task Get_OrderedByCreationTime()
    {
        for (var i = 0; i < 10; i++) await _db.CreateTopicAsync();

        var request = new GetTopicsRequest(new PageRequest(1, 10), TopicsOrderedBy.CreationTime);
        var response = await _topics.Get(request, CancellationToken.None);

        response.List.Should().BeInDescendingOrder(t => t.CreationTime);
    }

    [Fact]
    public async Task Get_OrderedByViewCount()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
        {
            var topic = await _db.CreateTopicAsync();
            _dbContext.Set<Topic>().Attach(topic);
            for (var j = 0; j < random.Next(100); j++) topic.IncrementViewCount();
        }

        await _dbContext.SaveChangesAsync();

        var request = new GetTopicsRequest(new PageRequest(1, 10), TopicsOrderedBy.ViewCount);
        var response = await _topics.Get(request, CancellationToken.None);

        response.List.Should().BeInDescendingOrder(t => t.ViewCount);
    }

    [Fact]
    public async Task Get_OrderedByComplaintCount()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
        {
            var topic = await _db.CreateTopicAsync();
            _dbContext.Set<Topic>().Attach(topic);
            for (var j = 0; j < random.Next(100); j++) topic.IncrementComplaintCount();
        }

        await _dbContext.SaveChangesAsync();

        var request = new GetTopicsRequest(new PageRequest(1, 10), TopicsOrderedBy.ComplaintCount);
        var response = await _topics.Get(request, CancellationToken.None);

        response.List.Should().BeInDescendingOrder(t => t.ComplaintCount);
    }

    [Fact]
    public async Task Get_OrderedByVerifyBefore()
    {
        var now = DateTime.UtcNow;
        for (var i = 0; i < 10; i++) await _db.CreateTopicAsync();

        var request = new GetTopicsRequest(new PageRequest(1, 10), TopicsOrderedBy.VerifyBefore);
        var response = await _topics.Get(request, CancellationToken.None);

        response.List.Should()
            .NotContain(t => t.VerifyBefore == null || t.VerifyBefore <= now).And
            .BeInAscendingOrder(t => t.VerifyBefore);
    }

    [Fact]
    public async Task GetById()
    {
        var topic = await _db.CreateTopicAsync();
        await FluentActions
            .Invoking(() => _topics.Get(topic.Id, CancellationToken.None))
            .Should()
            .NotThrowAsync();
    }

    [Fact]
    public async Task GetById_ThrowExceptionIfNotFound()
    {
        await FluentActions
            .Invoking(() => _topics.Get(Guid.NewGuid(), CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateTopic()
    {
        var request = CreateTopicRequest();

        var response = await _topics.Create(request, CancellationToken.None);
        var topic = await _dbContext.Set<Topic>().FindAsync(response.Id);

        topic.Should().NotBeNull();
        topic!.Header.Should().Be(request.Header);
        topic.Description.Should().Be(request.Description);
        topic.CompileOptions!.Code.Should().Be(topic.CompileOptions.Code);
        topic.CompileOptions.Language.Should().Be(topic.CompileOptions.Language);
        topic.IsClosed.Should().BeFalse();
        topic.VerifyBefore.Should().BeNull();
        topic.CreationTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task CreateTopic_ThrowExceptionIfTopicWithSameHeaderExist()
    {
        var topic = await _db.CreateTopicAsync();
        var request = CreateTopicRequest() with {Header = topic.Header};

        await FluentActions
            .Invoking(() => _topics.Create(request, CancellationToken.None))
            .Should()
            .ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task CreateTopic_ThrowExceptionIfSectionNotExist()
    {
        var request = CreateTopicRequest() with {SectionId = -1};

        await FluentActions
            .Invoking(() => _topics.Create(request, CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateTopic_ThrowExceptionIfUserNotExist()
    {
        var request = CreateTopicRequest() with {UserId = Guid.NewGuid()};

        await FluentActions
            .Invoking(() => _topics.Create(request, CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    private CreateTopicRequest CreateTopicRequest()
    {
        var fixture = new Fixture();
        return fixture.Create<CreateTopicRequest>() with
        {
            SectionId = _db.DefaultSection.Id,
            UserId = _db.DefaultUser.Id,
            CompileOptions = new CompileOptionsRequest(fixture.Create<string>(), "R"),
        };
    }
}
