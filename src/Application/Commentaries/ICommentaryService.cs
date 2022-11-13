using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Commentaries;

public interface ICommentaryService
{
    Task<Page<CommentaryResponse>> Get(GetCommentariesRequest request, CancellationToken cancellationToken);
    Task<CommentaryResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> Create(CreateCommentaryRequest request, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
}
