using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Users;
using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Topics;

public class TopicService : ITopicService
{
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<TopicService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _users;

    public TopicService(
        IAppDbContext dbContext,
        ILogger<TopicService> logger,
        IMapper mapper,
        IUserService users)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
        _users = users;
    }

    public async Task<IEnumerable<TopicInfoResponse>> Get(CancellationToken cancellationToken)
    {
        var topicInfos = await _dbContext.Set<Topic>()
            .ProjectTo<TopicInfoResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var users = await topicInfos
            .Select(t => _users.Get(t.UserId, cancellationToken))
            .WhenAllAsync();

        return topicInfos.Zip(users, (t, u) => t with {UserLogin = u.Login});
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

        var user = await _users.Get(topic.UserId, cancellationToken);

        return _mapper.Map<TopicResponse>(topic) with {UserLogin = user.Login};
    }

    public async Task<IdResponse<Guid>> Create(
        CreateTopicRequest request,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var isTopicExist = await _dbContext.Set<Topic>().AnyAsync(t => t.Header == request.Header, cancellationToken);
        if (isTopicExist) throw new ConflictException($"There is already topic with header: {request.Header}");

        var topic = new Topic(request.Header, request.Description, request.Code, userId);
        _dbContext.Set<Topic>().Add(topic);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created topic with id: {Id}", topic.Id);

        return new IdResponse<Guid>(topic.Id);
    }
}
