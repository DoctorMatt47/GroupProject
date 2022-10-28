using GroupProject.Domain.ValueObjects;

namespace GroupProject.Application.Commentaries;

public record CreateCommentaryRequest(
    string Description,
    CompileOptions CompileOptions,
    Guid TopicId,
    Guid UserId);
