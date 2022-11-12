using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Commentaries;

public interface ICommentaryService
{
    Task<CommentaryResponse> Get(Guid id, CancellationToken cancellationToken);

    Task<Page<CommentaryByUserResponse>> GetByUserIdOrderedByCreationTime(
        Guid id,
        PageRequest request,
        CancellationToken cancellationToken);

    Task<Page<CommentaryResponse>> GetByTopicIdOrderedByCreationTime(
        Guid id,
        PageRequest request,
        CancellationToken cancellationToken);

    Task<Page<CommentaryResponse>> GetOrderedByComplaintCount(
        PageRequest request,
        CancellationToken cancellationToken);

    Task<IdResponse<Guid>> Create(CreateCommentaryRequest request, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
}
