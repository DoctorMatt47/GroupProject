using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Topics;

public interface ITopicService
{
    Task<Page<TopicHeaderResponse>> Get(GetTopicsRequest request, CancellationToken cancellationToken);
    Task<TopicResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> Create(CreateTopicRequest request, CancellationToken cancellationToken);
    Task View(Guid id, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
    Task Close(Guid id, Guid userId, CancellationToken cancellationToken);
    Task Verify(Guid id, CancellationToken cancellationToken);
}
