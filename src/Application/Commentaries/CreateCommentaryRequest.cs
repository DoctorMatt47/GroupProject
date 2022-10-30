using GroupProject.Application.Common.Requests;

namespace GroupProject.Application.Commentaries;

public record CreateCommentaryRequest(
    string Description,
    CompileOptionsRequest? CompileOptions,
    Guid TopicId,
    Guid UserId);
