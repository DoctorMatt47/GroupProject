using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Topics;

public interface ITopicService
{
    Task<Page<TopicHeaderResponse>> GetOrderedByCreationTime(
        PageParameters parameters,
        CancellationToken cancellationToken);

    Task<Page<TopicHeaderResponse>> GetOrderedByComplaintCount(
        PageParameters parameters,
        CancellationToken cancellationToken);

    Task<Page<TopicHeaderResponse>> GetBySectionIdOrderedByCreationTime(
        int sectionId,
        PageParameters parameters,
        CancellationToken cancellationToken);

    Task<Page<TopicByUserIdResponse>> GetByUserIdOrderedByCreationTime(
        Guid userId,
        PageParameters parameters,
        CancellationToken cancellationToken);

    Task<TopicResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> Create(CreateTopicRequest request, CancellationToken cancellationToken);
    Task Close(Guid id, Guid userId, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
}
