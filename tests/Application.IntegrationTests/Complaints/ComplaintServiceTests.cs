using AutoFixture;
using FluentAssertions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Complaints;
using GroupProject.Application.IntegrationTests.Common.Fixtures;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.ValueObjects;

namespace GroupProject.Application.IntegrationTests.Complaints;

public class ComplaintServiceTests
{
    private readonly IComplaintService _complaints;
    private readonly DatabaseFixture _db;
    private readonly IAppDbContext _dbContext;

    public ComplaintServiceTests(
        DatabaseFixture db,
        IAppDbContext dbContext,
        IComplaintService complaints)
    {
        _db = db;
        _dbContext = dbContext;
        _complaints = complaints;
    }

    [Fact]
    public async Task GetByTopicId()
    {
        var topic = Topic();
        _dbContext.Set<Topic>().Add(topic);

        var fixture = new Fixture();
        var random = new Random();
        var complaintCount = random.Next(10);
        for (var i = 0; i < complaintCount; i++)
        {
            var complaint = new Complaint(
                fixture.Create<string>(),
                ComplaintTarget.Topic,
                topic.Id,
                _db.DefaultUser.Id,
                TimeSpan.Zero);

            _dbContext.Set<Complaint>().Add(complaint);
        }

        await _dbContext.SaveChangesAsync();

        var complaints = await _complaints.GetByTopicId(topic.Id, CancellationToken.None);
        complaints.Should().HaveCount(complaintCount);
    }

    [Fact]
    public async Task GetByCommentaryId()
    {
        var topic = Topic();
        _dbContext.Set<Topic>().Add(topic);

        var commentary = Commentary(topic.Id);

        _dbContext.Set<Commentary>().Add(commentary);

        var fixture = new Fixture();
        var random = new Random();
        var complaintCount = random.Next(10);
        for (var i = 0; i < complaintCount; i++)
        {
            var complaint = new Complaint(
                fixture.Create<string>(),
                ComplaintTarget.Commentary,
                commentary.Id,
                _db.DefaultUser.Id,
                TimeSpan.Zero);

            _dbContext.Set<Complaint>().Add(complaint);
        }

        await _dbContext.SaveChangesAsync();

        var complaints = await _complaints.GetByCommentaryId(commentary.Id, CancellationToken.None);
        complaints.Should().HaveCount(complaintCount);
    }

    private Commentary Commentary(Guid topicId)
    {
        var fixture = new Fixture();
        return new Commentary(
            fixture.Create<string>(),
            fixture.Create<CompileOptions>(),
            topicId,
            _db.DefaultUser.Id,
            fixture.Create<TimeSpan>());
    }

    private Topic Topic()
    {
        var fixture = new Fixture();
        return new Topic(
            fixture.Create<string>(),
            fixture.Create<string>(),
            fixture.Create<CompileOptions>(),
            _db.DefaultUser.Id,
            _db.DefaultSection.Id,
            fixture.Create<TimeSpan>());
    }
}
