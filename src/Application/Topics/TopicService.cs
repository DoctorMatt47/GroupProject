using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Configurations;
using GroupProject.Application.Phrases;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Topics;

public class TopicService : ITopicService
{
    private readonly IConfigurationService _configuration;
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<TopicService> _logger;
    private readonly IMapper _mapper;
    private readonly IPhraseService _phrases;

    public TopicService(
        IAppDbContext dbContext,
        IPhraseService phrases,
        IConfigurationService configuration,
        ILogger<TopicService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _phrases = phrases;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Page<TopicHeaderResponse>> Get(GetTopicsRequest request, CancellationToken cancellationToken)
    {
        var topics = _dbContext.Set<Topic>()
            .Include(t => t.Section)
            .Include(t => t.User)
            .AsQueryable();

        var (pageRequest, orderBy, onlyOpen, substring, sectionId, userId) = request;
        if (substring is not null) topics = topics.Where(t => t.Header.Contains(substring));
        if (sectionId is not null) topics = topics.Where(t => t.SectionId == sectionId);
        if (userId is not null) topics = topics.Where(t => t.UserId == userId);
        if (onlyOpen) topics = topics.Where(Topic.IsOpen);

        topics = orderBy switch
        {
            TopicsOrderedBy.CreationTime => topics
                .OrderByDescending(t => t.CreationTime),

            TopicsOrderedBy.ViewCount => topics
                .OrderByDescending(t => t.ViewCount),

            TopicsOrderedBy.ComplaintCount => topics
                .Where(t => t.ComplaintCount != 0)
                .OrderByDescending(t => t.ComplaintCount),

            TopicsOrderedBy.VerifyBefore => topics
                .Where(Topic.VerificationRequired)
                .OrderBy(t => t.VerifyBefore),

            _ => throw new ArgumentOutOfRangeException(),
        };

        return await topics
            .ProjectTo<TopicHeaderResponse>(_mapper.ConfigurationProvider)
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

        await ThrowIfContainsForbiddenPhrasesAsync(request, cancellationToken);

        var section = await _dbContext.Set<Section>().FindOrThrowAsync(request.SectionId, cancellationToken);
        section.IncrementTopicCount();

        var verificationDuration = await VerificationDurationOrNullAsync(request, cancellationToken);

        var compileOptions = _mapper.Map<CompileOptions>(request.CompileOptions);
        var topic = new Topic(
            request.Header,
            request.Description,
            compileOptions,
            request.UserId,
            request.SectionId,
            verificationDuration);

        _dbContext.Set<Topic>().Add(topic);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created topic with id: {Id}", topic.Id);

        return new IdResponse<Guid>(topic.Id);
    }

    public async Task View(Guid id, CancellationToken cancellationToken)
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
        // TODO: Add is already verified check
        topic.SetVerified();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Close(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>().FindOrThrowAsync(userId, cancellationToken);
        var topic = await _dbContext.Set<Topic>().FindOrThrowAsync(id, cancellationToken);

        if (user.Role is UserRole.User && topic.UserId != user.Id)
            throw new ForbiddenException("You don't have permission for closing this topic");

        topic.SetClosed();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ThrowIfContainsForbiddenPhrasesAsync(
        CreateTopicRequest request,
        CancellationToken cancellationToken)
    {
        var forbiddenPhrases = await _phrases.GetForbidden(
            p => request.Header.Contains(p.Phrase) || request.Description.Contains(p.Phrase),
            cancellationToken);

        if (!forbiddenPhrases.Any()) return;
        throw new BadRequestException($"Topic contains forbidden words: {string.Join(',', forbiddenPhrases)}");
    }

    private async Task<TimeSpan?> VerificationDurationOrNullAsync(
        CreateTopicRequest request,
        CancellationToken cancellationToken)
    {
        var verificationRequired = await _phrases.AnyVerificationRequired(
            p => request.Header.Contains(p.Phrase) || request.Description.Contains(p.Phrase),
            cancellationToken);

        if (!verificationRequired) return null;

        var configuration = await _configuration.Get(cancellationToken);
        return configuration.VerificationDuration;
    }
}
