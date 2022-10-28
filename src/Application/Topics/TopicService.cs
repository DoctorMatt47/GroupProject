﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Responses;
using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Topics;

public class TopicService : ITopicService
{
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<TopicService> _logger;
    private readonly IMapper _mapper;

    public TopicService(
        IAppDbContext dbContext,
        ILogger<TopicService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Page<TopicInfoResponse>> GetOrderedByCreationTime(
        int perPage,
        int page,
        CancellationToken cancellationToken)
    {
        var pageCount = await _dbContext.Set<Topic>().PageCountAsync(perPage, cancellationToken);

        return await _dbContext.Set<Topic>()
            .OrderByDescending(t => t.CreationTime)
            .ProjectTo<TopicInfoResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(perPage, page, pageCount, cancellationToken);
    }

    public async Task<Page<TopicInfoResponse>> GetOrderedByComplaintCount(
        int perPage,
        int page,
        CancellationToken cancellationToken)
    {
        var pageCount = await _dbContext.Set<Topic>().PageCountAsync(perPage, cancellationToken);

        return await _dbContext.Set<Topic>()
            .Include(t => t.Complaints)
            .Where(t => t.ComplaintCount != 0)
            .OrderBy(t => t.ComplaintCount)
            .ProjectTo<TopicInfoResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(perPage, page, pageCount, cancellationToken);
    }

    public async Task<IEnumerable<TopicByUserIdResponse>> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Topic>()
            .Where(t => t.UserId == userId)
            .ProjectTo<TopicByUserIdResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<TopicResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var topic = await _dbContext.Set<Topic>().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (topic is null) throw new NotFoundException($"There is no topic with id: {id}");

        return _mapper.Map<TopicResponse>(topic);
    }

    public async Task<IdResponse<Guid>> Create(CreateTopicRequest request, CancellationToken cancellationToken)
    {
        var isUserExist = await _dbContext.Set<User>().AnyAsync(u => u.Id == request.UserId, cancellationToken);
        if (!isUserExist) throw new NotFoundException($"There is no user with id: {request.UserId}");

        var isSectionExist = await _dbContext.Set<Section>().AnyAsync(
            s => s.Id == request.SectionId,
            cancellationToken);

        if (!isSectionExist) throw new NotFoundException($"There is no section with id: {request.SectionId}");

        var isTopicExist = await _dbContext.Set<Topic>().AnyAsync(t => t.Header == request.Header, cancellationToken);
        if (isTopicExist) throw new ConflictException($"There is already topic with header: {request.Header}");

        var topic = new Topic(
            request.Header,
            request.Description,
            request.CompileOptions,
            request.UserId,
            request.SectionId);

        _dbContext.Set<Topic>().Add(topic);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created topic with id: {Id}", topic.Id);

        return new IdResponse<Guid>(topic.Id);
    }
}
