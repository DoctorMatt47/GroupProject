using AutoFixture;
using FluentAssertions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.IntegrationTests.Common.Fixtures;
using GroupProject.Application.Topics;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.ValueObjects;

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
    public async Task GetOrderedByCreationTime()
    {
        for (var i = 0; i < 10; i++)
        {
            var topic = Topic();
            _dbContext.Set<Topic>().Add(topic);
        }

        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var topicPage = await _topics.GetOrderedByCreationTime(10, 1, CancellationToken.None);
        var topics = topicPage.List.ToList();

        for (var i = 0; i < topics.Count - 1; i++)
        {
            topics[i].CreationTime.Should().BeOnOrBefore(topics[i + 1].CreationTime);
        }
    }

    [Fact]
    public async Task GetOrderedByComplaintCount()
    {
        var fixture = new Fixture();
        var random = new Random();

        for (var i = 0; i < 10; i++)
        {
            var topic = Topic();
            _dbContext.Set<Topic>().Add(topic);

            for (var j = 0; j < random.Next(5); j++)
            {
                var complaint = new Complaint(fixture.Create<string>(), ComplaintTarget.Topic, topic.Id);
                _dbContext.Set<Complaint>().Add(complaint);
            }
        }

        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var topicPage = await _topics.GetOrderedByComplaintCount(10, 1, CancellationToken.None);
        var topics = topicPage.List.ToList();

        for (var i = 0; i < topics.Count - 1; i++)
        {
            topics[i].ComplaintCount.Should().NotBe(0).And.BeLessOrEqualTo(topics[i + 1].ComplaintCount);
        }
    }

    [Fact]
    public async Task GetById()
    {
        var topic = Topic();

        _dbContext.Set<Topic>().Add(topic);
        await _dbContext.SaveChangesAsync();

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
        topic.IsVerificationRequired.Should().BeFalse();
        topic.CreationTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task CreateTopic_ThrowExceptionIfTopicWithSameHeaderExist()
    {
        var topic = Topic();
        var request = CreateTopicRequest() with {Header = topic.Header};

        _dbContext.Set<Topic>().Add(topic);
        await _dbContext.SaveChangesAsync();

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
        };
    }

    private Topic Topic()
    {
        var fixture = new Fixture();

        return new Topic(
            fixture.Create<string>(),
            fixture.Create<string>(),
            fixture.Create<CompileOptions>(),
            _db.DefaultUser.Id,
            _db.DefaultSection.Id);
    }
}
