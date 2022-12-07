using FluentAssertions;
using GroupProject.Application.Commentaries;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Requests;
using GroupProject.Application.IntegrationTests.Common.Fixtures;
using GroupProject.Application.IntegrationTests.Common.Utils;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.IntegrationTests.Commentaries;

public class CommentaryServiceTests
{
    private readonly ICommentaryService _commentaries;
    private readonly DatabaseFixture _db;
    private readonly IAppDbContext _dbContext;

    public CommentaryServiceTests(DatabaseFixture db, IAppDbContext dbContext, ICommentaryService commentaries)
    {
        _db = db;
        _dbContext = dbContext;
        _commentaries = commentaries;
    }

    [Fact]
    public async Task Get_CommentariesOrderedByComplaintCount()
    {
        var random = new Random();

        var topic = await _db.CreateTopicAsync();
        _dbContext.Set<Topic>().Attach(topic);

        var setComplaintCount = ReflectionUtil.GetPrivateSetProperty<Commentary>(nameof(Commentary.ComplaintCount));
        for (var i = 0; i < 10; i++)
        {
            var commentary = await _db.CreateCommentaryAsync(topic.Id);
            _dbContext.Set<Commentary>().Attach(commentary);
            setComplaintCount!.SetValue(commentary, random.Next(100));
        }

        await _dbContext.SaveChangesAsync();

        var request = new GetCommentariesRequest(new PageRequest(1, 10), CommentariesOrderedBy.ComplaintCount);
        var response = await _commentaries.Get(request, CancellationToken.None);

        response.Items.Should()
            .OnlyContain(commentary => commentary.ComplaintCount > 0).And
            .BeInDescendingOrder(commentary => commentary.ComplaintCount);
    }

    [Fact]
    public async Task Get_CommentariesOrderedByVerifyBefore()
    {
        var now = DateTime.UtcNow;
        var topic = await _db.CreateTopicAsync();
        _dbContext.Set<Topic>().Attach(topic);

        for (var i = 0; i < 10; i++)
        {
            var commentary = await _db.CreateCommentaryAsync(topic.Id);
            _dbContext.Set<Commentary>().Attach(commentary);
        }

        await _dbContext.SaveChangesAsync();

        var request = new GetCommentariesRequest(new PageRequest(1, 10), CommentariesOrderedBy.VerifyBefore);
        var response = await _commentaries.Get(request, CancellationToken.None);

        response.Items.Should()
            .OnlyContain(commentary => commentary.VerifyBefore != null && commentary.VerifyBefore > now).And
            .BeInAscendingOrder(commentary => commentary.VerifyBefore);
    }

    [Fact]
    public async Task VerifyCommentary()
    {
        var topic = await _db.CreateTopicAsync();
        var commentary = await _db.CreateCommentaryAsync(topic.Id);

        await _commentaries.Verify(commentary.Id, CancellationToken.None);

        commentary = await _dbContext.Set<Commentary>().FindAsync(topic.Id);
        commentary!.VerifyBefore.Should().BeNull();
    }

    [Fact]
    public async Task VerifyCommentary_ThrowExceptionIfNotFound()
    {
        await FluentActions
            .Invoking(() => _commentaries.Verify(Guid.NewGuid(), CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}

