﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;
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

    public TopicService(
        IAppDbContext dbContext,
        ILogger<TopicService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Page<TopicHeaderResponse>> GetOrderedByComplaintCount(
        PageParameters parameters,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Topic>()
            .Include(t => t.Section)
            .Include(t => t.User)
            .Where(t => t.ComplaintCount != 0)
            .OrderBy(t => t.ComplaintCount)
            .ProjectTo<TopicHeaderResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(parameters, cancellationToken);
    }

    public async Task<Page<TopicHeaderResponse>> GetOrderedByCreationTime(
        PageParameters parameters,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Topic>()
            .Include(t => t.Section)
            .Include(t => t.User)
            .OrderByDescending(t => t.CreationTime)
            .ProjectTo<TopicHeaderResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(parameters, cancellationToken);
    }

    public async Task<Page<TopicHeaderResponse>> GetBySectionIdOrderedByCreationTime(
        int sectionId,
        PageParameters parameters,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Topic>()
            .Include(t => t.Section)
            .Include(t => t.User)
            .Where(t => t.Section.Id == sectionId)
            .OrderByDescending(t => t.CreationTime)
            .ProjectTo<TopicHeaderResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(parameters, cancellationToken);
    }

    public async Task<Page<TopicByUserIdResponse>> GetByUserIdOrderedByCreationTime(
        Guid userId,
        PageParameters parameters,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Topic>()
            .Include(t => t.Section)
            .Include(t => t.User)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreationTime)
            .ProjectTo<TopicByUserIdResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(parameters, cancellationToken);
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
        section.IncrementTopicCount();

        var compileOptions = _mapper.Map<CompileOptions>(request.CompileOptions);
        var topic = new Topic(
            request.Header,
            request.Description,
            compileOptions,
            request.UserId,
            request.SectionId);

        _dbContext.Set<Topic>().Add(topic);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created topic with id: {Id}", topic.Id);

        return new IdResponse<Guid>(topic.Id);
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

    public async Task Close(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>().FindOrThrowAsync(userId, cancellationToken);
        var topic = await _dbContext.Set<Topic>().FindOrThrowAsync(id, cancellationToken);

        if (user.Role is UserRole.User && topic.UserId != user.Id)
            throw new ForbiddenException("You don't have permission for closing this topic");

        topic.IsClosed = true;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
