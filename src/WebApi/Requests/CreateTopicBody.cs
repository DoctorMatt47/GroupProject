using GroupProject.Domain.ValueObjects;

namespace GroupProject.WebApi.Requests;

public record CreateTopicBody(
    string Header,
    string Description,
    CompileOptions? CompileOptions);
