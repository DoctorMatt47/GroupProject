namespace GroupProject.WebApi.Requests;

public record CreateCommentaryBody(
    string Description,
    CompileOptionsBody? CompileOptions);
