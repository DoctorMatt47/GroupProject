using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Configurations;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Complaints;

public class ComplaintService : IComplaintService
{
    private readonly IConfigurationService _configuration;
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<ComplaintService> _logger;
    private readonly IMapper _mapper;

    public ComplaintService(
        IAppDbContext dbContext,
        IConfigurationService configuration,
        ILogger<ComplaintService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Page<ComplaintResponse>> Get(
        PageRequest request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Complaint>()
            .Where(Complaint.Active)
            .OrderByDescending(c => c.CreationTime)
            .ProjectTo<ComplaintResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(request, cancellationToken);
    }

    public async Task<ComplaintResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var complaint = await _dbContext.Set<Complaint>().FindOrThrowAsync(id, cancellationToken);
        return _mapper.Map<ComplaintResponse>(complaint);
    }

    public async Task<IEnumerable<ComplaintByTargetResponse>> GetByTopicId(
        Guid topicId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Complaint>()
            .Where(Complaint.Active)
            .Where(c => c.TopicId == topicId)
            .ProjectTo<ComplaintByTargetResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ComplaintByTargetResponse>> GetByCommentaryId(
        Guid commentaryId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Complaint>()
            .Where(Complaint.Active)
            .Where(c => c.CommentaryId == commentaryId)
            .ProjectTo<ComplaintByTargetResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<IdResponse<Guid>> Create(
        CreateComplaintRequest request,
        CancellationToken cancellationToken)
    {
        var target = await FindTargetOrThrowAsync(request.Target, request.TargetId, cancellationToken);
        target.IncrementComplaintCount();

        var configuration = await _configuration.Get(cancellationToken);
        var complaint = new Complaint(
            request.Description,
            request.Target,
            request.TargetId,
            request.UserId,
            configuration.VerificationDuration);

        _dbContext.Set<Complaint>().Add(complaint);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Created complaint with id: {ComplaintId} on {Target}: {TopicId}",
            complaint.Id,
            Enum.GetName(request.Target),
            request.TargetId);

        return new IdResponse<Guid>(complaint.Id);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var complaint = await _dbContext.Set<Complaint>().FindOrThrowAsync(id, cancellationToken);
        var targetId = (Guid) (complaint.CommentaryId ?? complaint.TopicId)!;
        var target = await FindTargetOrThrowAsync(complaint.Target, targetId, cancellationToken);

        target.DecrementComplaintCount();

        _dbContext.Set<Complaint>().Remove(complaint);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<IHasComplaintCount> FindTargetOrThrowAsync(
        ComplaintTarget target,
        Guid targetId,
        CancellationToken cancellationToken)
    {
        return target switch
        {
            ComplaintTarget.Topic =>
                await _dbContext.Set<Topic>().FindOrThrowAsync(targetId, cancellationToken),

            ComplaintTarget.Commentary =>
                await _dbContext.Set<Commentary>().FindOrThrowAsync(targetId, cancellationToken),

            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
