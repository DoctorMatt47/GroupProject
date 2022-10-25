using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Responses;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Commentaries;

public class CommentaryService : ICommentaryService
{
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<CommentaryService> _logger;
    private readonly IMapper _mapper;

    public CommentaryService(
        IAppDbContext dbContext,
        ILogger<CommentaryService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Page<CommentaryResponse>> GetByTopicId(
        Guid id,
        int perPage,
        int page,
        CancellationToken cancellationToken)
    {
        var pageCount = await _dbContext.Set<Commentary>().PageCountAsync(perPage, cancellationToken);

        return await _dbContext.Set<Commentary>()
            .Where(c => c.TopicId == id)
            .OrderBy(c => c.CreationTime)
            .ProjectTo<CommentaryResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(perPage, page, pageCount, cancellationToken);
    }

    public async Task<IdResponse<Guid>> Create(
        CreateCommentaryRequest request,
        CancellationToken cancellationToken)
    {
        var topic = await _dbContext.Set<Topic>().FirstOrDefaultAsync(t => t.Id == request.TopicId, cancellationToken);
        if (topic is null) throw new NotFoundException($"There is no topic with id: {request.TopicId}");
        if (topic.Status == TopicStatus.Closed) throw new ConflictException("Topic has been closed");

        var commentary = new Commentary(request.Description, request.Code, request.TopicId, request.UserId);
        _dbContext.Set<Commentary>().Add(commentary);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Created commentary with id: {CommentaryId} on topic: {TopicId}",
            commentary.Id, commentary.TopicId);

        return new IdResponse<Guid>(commentary.Id);
    }
}
