using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Commentaries;

public interface ICommentaryService
{
    Task<IEnumerable<CommentaryResponse>> GetByTopicId(Guid id, CancellationToken cancellationToken);

    Task<IdResponse<Guid>> Create(
        CreateCommentaryRequest request,
        Guid userId,
        CancellationToken cancellationToken);
}