using GroupProject.Domain.ValueObjects;

namespace GroupProject.Application.Commentaries;

public record CommentaryResponse(
    Guid Id,
    string Description,
    CompileOptions CompileOptions,
    Guid UserId,
    Guid TopicId,
    DateTime CreationTime);
