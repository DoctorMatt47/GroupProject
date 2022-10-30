using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Commentaries;

public interface ICommentaryService
{
    Task<Page<CommentaryResponse>> GetByTopicIdOrderedByCreationTime(
        Guid id,
        int perPage,
        int page,
        CancellationToken cancellationToken);

    Task<Page<CommentaryResponse>> GetOrderedByComplaintCount(
        int perPage,
        int page,
        CancellationToken cancellationToken);

    Task<IdResponse<Guid>> Create(CreateCommentaryRequest request, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
}
