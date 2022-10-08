using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Commentaries;

public interface ICommentaryService
{
    Task<Page<CommentaryResponse>> GetByTopicId(Guid id, int perPage, int page, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> Create(CreateCommentaryRequest request, CancellationToken cancellationToken);
}
