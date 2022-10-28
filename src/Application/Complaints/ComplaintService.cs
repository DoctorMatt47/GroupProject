using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Responses;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Complaints;

public class ComplaintService : IComplaintService
{
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<ComplaintService> _logger;
    private readonly IMapper _mapper;

    public ComplaintService(
        IAppDbContext dbContext,
        ILogger<ComplaintService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ComplaintResponse>> GetByTopicId(
        Guid topicId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Complaint>()
            .Where(c => c.TopicId == topicId)
            .ProjectTo<ComplaintResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ComplaintResponse>> GetByCommentaryId(
        Guid commentaryId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Complaint>()
            .Where(c => c.CommentaryId == commentaryId)
            .ProjectTo<ComplaintResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<IdResponse<Guid>> CreateComplaint(
        CreateComplaintRequest request,
        CancellationToken cancellationToken)
    {
        var target = await GetTargetAsync(request, cancellationToken);
        if (target is null)
            throw new NotFoundException($"There is no {Enum.GetName(request.Target)} with id: {request.ElementId}");

        target.IncrementComplaintCount();

        var complaint = new Complaint(request.Description, request.Target, request.ElementId);

        _dbContext.Set<Complaint>().Add(complaint);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Created complaint with id: {ComplaintId} on {Target}: {TopicId}",
            complaint.Id,
            Enum.GetName(request.Target),
            request.ElementId);

        return new IdResponse<Guid>(complaint.Id);
    }

    private async Task<IHasComplaintCount?> GetTargetAsync(
        CreateComplaintRequest request,
        CancellationToken cancellationToken)
    {
        return request.Target switch
        {
            ComplaintTarget.Topic => await _dbContext.Set<Topic>()
                .FirstOrDefaultAsync(t => t.Id == request.ElementId, cancellationToken),

            ComplaintTarget.Commentary => await _dbContext.Set<Commentary>()
                .FirstOrDefaultAsync(t => t.Id == request.ElementId, cancellationToken),

            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
