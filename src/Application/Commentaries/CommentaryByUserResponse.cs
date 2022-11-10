using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Commentaries;

public record CommentaryByUserResponse(
    Guid Id,
    string Description,
    CompileOptionsResponse? CompileOptions,
    Guid TopicId,
    DateTime CreationTime);
