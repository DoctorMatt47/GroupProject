using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Commentaries;

public record CommentaryResponse(
    Guid Id,
    string Description,
    CompileOptionsResponse? CompileOptions,
    Guid UserId,
    Guid TopicId,
    DateTime CreationTime);
