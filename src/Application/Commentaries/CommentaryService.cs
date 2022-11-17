using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Configurations;
using GroupProject.Application.Phrases;
using GroupProject.Domain.Entities;
using GroupProject.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Commentaries;

public class CommentaryService : ICommentaryService
{
    private readonly IConfigurationService _configuration;
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<CommentaryService> _logger;
    private readonly IMapper _mapper;
    private readonly IPhraseService _phrases;

    public CommentaryService(
        IAppDbContext dbContext,
        IConfigurationService configuration,
        IPhraseService phrases,
        ILogger<CommentaryService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _phrases = phrases;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Page<CommentaryResponse>> Get(GetCommentariesRequest request, CancellationToken cancellationToken)
    {
        var (pageRequest, orderBy, topicId, userId) = request;
        if (topicId is null && userId is null)
            throw new BadRequestException("Topic id and user id should not be null at the same time");

        var commentaries = _dbContext.Set<Commentary>()
            .Include(c => c.User)
            .AsQueryable();

        if (topicId is not null) commentaries = commentaries.Where(c => c.TopicId == topicId);
        if (userId is not null) commentaries = commentaries.Where(c => c.UserId == userId);

        commentaries = orderBy switch
        {
            CommentariesOrderedBy.CreationTime => commentaries
                .OrderByDescending(c => c.CreationTime),

            CommentariesOrderedBy.ComplaintCount => commentaries
                .Where(c => c.ComplaintCount != 0)
                .OrderByDescending(c => c.ComplaintCount),

            CommentariesOrderedBy.VerifyBefore => commentaries
                .Where(Commentary.VerificationRequired)
                .OrderBy(c => c.VerifyBefore),

            _ => throw new ArgumentOutOfRangeException(),
        };

        return await commentaries
            .ProjectTo<CommentaryResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(pageRequest, cancellationToken);
    }

    public async Task<CommentaryResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var commentary = await _dbContext.Set<Commentary>()
            .Include(c => c.User)
            .FirstOrThrowAsync(id, cancellationToken);

        return _mapper.Map<CommentaryResponse>(commentary);
    }

    public async Task<IdResponse<Guid>> Create(
        CreateCommentaryRequest request,
        CancellationToken cancellationToken)
    {
        var topic = await _dbContext.Set<Topic>().FindOrThrowAsync(request.TopicId, cancellationToken);
        if (topic.IsClosed) throw new ConflictException("Topic has been closed");

        await ThrowIfContainsForbiddenPhrasesOrNull();

        var verificationDuration = await VerificationDurationOrNullAsync();

        var compileOptions = _mapper.Map<CompileOptions>(request.CompileOptions);
        var commentary = new Commentary(
            request.Description,
            compileOptions,
            request.TopicId,
            request.UserId,
            verificationDuration);

        _dbContext.Set<Commentary>().Add(commentary);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Created commentary with id: {CommentaryId} on topic: {TopicId}",
            commentary.Id,
            commentary.TopicId);

        return new IdResponse<Guid>(commentary.Id);

        async Task<TimeSpan?> VerificationDurationOrNullAsync()
        {
            var verificationRequired = (await _phrases
                    .GetVerificationRequired(cancellationToken))
                .Any(p =>
                    _phrases.ContainsPhrase(request.CompileOptions?.Code ?? string.Empty, p.Phrase) ||
                    _phrases.ContainsPhrase(request.Description, p.Phrase));

            if (!verificationRequired) return null;

            var configuration = await _configuration.Get(cancellationToken);
            return configuration.VerificationDuration;
        }

        async Task ThrowIfContainsForbiddenPhrasesOrNull()
        {
            var forbiddenPhrases = (await _phrases
                    .GetForbidden(cancellationToken))
                .Select(p => p.Phrase)
                .Where(phrase =>
                    _phrases.ContainsPhrase(request.CompileOptions?.Code ?? string.Empty, phrase) ||
                    _phrases.ContainsPhrase(request.Description, phrase))
                .ToList();

            if (!forbiddenPhrases.Any()) return;
            throw new BadRequestException($"Topic contains forbidden words: {string.Join(',', forbiddenPhrases)}");
        }
    }

    public async Task Verify(Guid id, CancellationToken cancellationToken)
    {
        var commentary = await _dbContext.Set<Commentary>().FindOrThrowAsync(id, cancellationToken);
        // TODO: Add is already verified check
        commentary.SetVerified();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var commentary = await _dbContext.Set<Commentary>().FindOrThrowAsync(id, cancellationToken);
        _dbContext.Set<Commentary>().Remove(commentary);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
