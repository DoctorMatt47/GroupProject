using GroupProject.Application.Common.Requests;

namespace GroupProject.WebApi.Requests;

public record CreateCommentaryBody(
    string Description,
    CompileOptionsRequest? CompileOptions);
