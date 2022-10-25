﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Responses;
using GroupProject.Domain.Entities;
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

    public async Task<IEnumerable<ComplaintResponse>> GetByTopicId(Guid topicId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Complaint>()
            .Where(c => c.Id == topicId)
            .ProjectTo<ComplaintResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<IdResponse<Guid>> CreateComplaint(
        CreateComplaintRequest request,
        CancellationToken cancellationToken)
    {
        var isTopicExist = await _dbContext.Set<Topic>().AnyAsync(t => t.Id == request.TopicId, cancellationToken);
        if (!isTopicExist) throw new NotFoundException($"There is no topic with id: {request.TopicId}");

        var complaint = new Complaint(request.Description, request.TopicId);

        _dbContext.Set<Complaint>().Add(complaint);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Created complaint with id: {ComplaintId} on topic: {TopicId}",
            complaint.Id,
            request.TopicId);

        return new IdResponse<Guid>(complaint.Id);
    }
}
