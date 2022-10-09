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
    private readonly IAppDbContext _context;
    private readonly ILogger<TopicService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _users;

    public TopicService(
        IAppDbContext context,
        ILogger<TopicService> logger,
        IMapper mapper,
        IUserService users)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _users = users;
    }

    public async Task<Page<TopicInfoForUserResponse>> GetOrderedByCreationTime(
        int perPage,
        int page,
        CancellationToken cancellationToken)
    {
        var pageCount = await _context.Set<Topic>().PageCountAsync(perPage, cancellationToken);

        var topics = await _context.Set<Topic>()
            .OrderByDescending(t => t.CreationTime)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync(cancellationToken);

        var users = await topics
            .Select(t => _users.Get(t.UserId, cancellationToken))
            .WhenAllAsync();

        return topics
            .Zip(users, (topic, user) => _mapper.Map<TopicInfoForUserResponse>(topic) with {UserLogin = user.Login})
            .ToPage(pageCount);
    }

    public async Task<Page<TopicInfoForModeratorResponse>> GetOrderedByComplaintCount(
        int perPage,
        int page,
        CancellationToken cancellationToken)
    {
        var pageCount = await _context.Set<Topic>().PageCountAsync(perPage, cancellationToken);

        var topics = await _context.Set<Topic>()
            .Include(t => t.Complaints)
            .OrderBy(t => t.Complaints.Count())
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync(cancellationToken);

        var users = await topics
            .Select(t => _users.Get(t.UserId, cancellationToken))
            .WhenAllAsync();

        return topics
            .Zip(users, (topic, user) => _mapper.Map<TopicInfoForModeratorResponse>(topic) with
            {
                UserLogin = user.Login,
                ComplaintCount = topic.Complaints.Count(),
            })
            .ToPage(pageCount);
    }

    public async Task<IEnumerable<TopicByUserIdResponse>> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Set<Topic>()
            .Where(t => t.UserId == userId)
            .ProjectTo<TopicByUserIdResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<TopicResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var topic = await _context.Set<Topic>().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (topic is null) throw new NotFoundException($"There is no topic with id: {id}");

        var user = await _users.Get(topic.UserId, cancellationToken);

        return _mapper.Map<TopicResponse>(topic) with {UserLogin = user.Login};
    }

    public async Task<IdResponse<Guid>> Create(CreateTopicRequest request, CancellationToken cancellationToken)
    {
        var isUserExist = await _context.Set<User>().AnyAsync(u => u.Id == request.UserId, cancellationToken);
        if (!isUserExist) throw new NotFoundException($"There is no user with id: {request.UserId}");

        var isTopicExist = await _context.Set<Topic>().AnyAsync(t => t.Header == request.Header, cancellationToken);
        if (isTopicExist) throw new ConflictException($"There is already topic with header: {request.Header}");

        var topic = new Topic(request.Header, request.Description, request.Code, request.UserId);

        _context.Set<Topic>().Add(topic);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created topic with id: {Id}", topic.Id);

        return new IdResponse<Guid>(topic.Id);
    }
}
