using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Phrases;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Topics;

public class TopicService : ITopicService
{
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<TopicService> _logger;
    private readonly IMapper _mapper;
    private readonly IPhraseService _phrases;

    public TopicService(
        IAppDbContext dbContext,
        IPhraseService phrases,
        ILogger<TopicService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _phrases = phrases;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Page<TopicResponse>> Get(GetTopicsRequest request, CancellationToken cancellationToken)
    {
        var topics = _dbContext.Set<Topic>()
            .Include(t => t.Section)
            .Include(t => t.User)
            .AsQueryable();

        var (substring, sectionId, userId, orderedBy, pageRequest, isVerificationRequired, isOpen) = request;
        if (substring is not null) topics = topics.Where(t => t.Header.Contains(substring));
        if (sectionId is not null) topics = topics.Where(t => t.SectionId == sectionId);
        if (userId is not null) topics = topics.Where(t => t.UserId == userId);
        if (isVerificationRequired) topics = topics.Where(t => t.VerificationRequiredBefore != null);
        if (isOpen) topics = topics.Where(Topic.IsOpen);

        topics = orderedBy switch
        {
            TopicsOrderedByParameter.CreationTime => topics.OrderByDescending(t => t.CreationTime),
            TopicsOrderedByParameter.ViewCount => topics.OrderByDescending(t => t.ViewCount),
            TopicsOrderedByParameter.ComplaintCount => topics
                .Where(t => t.ComplaintCount != 0)
                .OrderByDescending(t => t.ComplaintCount),

            _ => throw new ArgumentOutOfRangeException(),
        };

        return await topics
            .ProjectTo<TopicResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(pageRequest, cancellationToken);
    }

    public async Task<TopicResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var topic = await _dbContext.Set<Topic>()
            .Include(t => t.Section)
            .Include(t => t.User)
            .FirstOrThrowAsync(id, cancellationToken);

        return _mapper.Map<TopicResponse>(topic);
    }

    public async Task<IdResponse<Guid>> Create(CreateTopicRequest request, CancellationToken cancellationToken)
    {
        await _dbContext.Set<User>().AnyOrThrowAsync(request.UserId, cancellationToken);
        await _dbContext.Set<Topic>().NoOneOrThrowAsync(t => t.Header == request.Header, cancellationToken);

        var section = await _dbContext.Set<Section>().FindOrThrowAsync(request.SectionId, cancellationToken);

        var forbiddenPhrases = await _phrases.GetForbiddenWhere(
            p => request.Header.Contains(p.Phrase) || request.Description.Contains(p.Phrase),
            cancellationToken);

        if (forbiddenPhrases.Any())
            throw new BadRequestException($"Topic contains forbidden words: {string.Join(',', forbiddenPhrases)}");

        section.IncrementTopicCount();

        var compileOptions = _mapper.Map<CompileOptions>(request.CompileOptions);
        var topic = new Topic(
            request.Header,
            request.Description,
            compileOptions,
            request.UserId,
            request.SectionId);

        var verificationRequired = await ContainsVerificationRequiredPhrases(request, cancellationToken);
        if (verificationRequired)
        {
            var configuration = await _dbContext.Set<Configuration>().FirstAsync(cancellationToken);
            topic.RequireVerification(configuration.VerificationDuration);
        }

        _dbContext.Set<Topic>().Add(topic);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created topic with id: {Id}", topic.Id);

        return new IdResponse<Guid>(topic.Id);
    }

    public async Task IncrementViewCount(Guid id, CancellationToken cancellationToken)
    {
        var topic = await _dbContext.Set<Topic>().FindOrThrowAsync(id, cancellationToken);
        topic.IncrementViewCount();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var topic = await _dbContext.Set<Topic>()
            .Include(t => t.Section)
            .FirstOrThrowAsync(id, cancellationToken);

        topic.Section.DecrementTopicCount();

        _dbContext.Set<Topic>().Remove(topic);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Verify(Guid id, CancellationToken cancellationToken)
    {
        var topic = await _dbContext.Set<Topic>().FindOrThrowAsync(id, cancellationToken);
        topic.Verify();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Close(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>().FindOrThrowAsync(userId, cancellationToken);
        var topic = await _dbContext.Set<Topic>().FindOrThrowAsync(id, cancellationToken);

        if (user.Role is UserRole.User && topic.UserId != user.Id)
            throw new ForbiddenException("You don't have permission for closing this topic");

        topic.Close();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<bool> ContainsVerificationRequiredPhrases(
        CreateTopicRequest request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<VerificationRequiredPhrase>().AnyAsync(
            p => request.Header.Contains(p.Phrase) || request.Description.Contains(p.Phrase),
            cancellationToken);
    }
}
