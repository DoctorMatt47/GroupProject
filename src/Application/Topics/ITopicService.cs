using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Topics;

public interface ITopicService
{
    Task<Page<TopicInfoResponse>> GetOrderedByCreationTime(
        int perPage,
        int page,
        CancellationToken cancellationToken);

    Task<Page<TopicInfoResponse>> GetOrderedByComplaintCount(
        int perPage,
        int page,
        CancellationToken cancellationToken);

    Task<TopicResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TopicByUserIdResponse>> GetByUserId(Guid userId, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> Create(CreateTopicRequest request, CancellationToken cancellationToken);
}
