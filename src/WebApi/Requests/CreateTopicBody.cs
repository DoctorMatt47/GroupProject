using GroupProject.Application.Common.Requests;

namespace GroupProject.WebApi.Requests;

public record CreateTopicBody(
    string Header,
    string Description,
    CompileOptionsRequest? CompileOptions);
