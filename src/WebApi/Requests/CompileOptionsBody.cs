namespace GroupProject.WebApi.Requests;

public record CompileOptionsBody(
    string Code,
    string Language);
