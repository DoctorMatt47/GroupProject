namespace GroupProject.WebApi.Requests;

public record CreateTopicBody(
    string Header,
    string Description,
    CompileOptionsBody? CompileOptions);
