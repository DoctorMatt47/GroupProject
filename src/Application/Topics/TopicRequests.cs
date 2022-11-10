using GroupProject.Application.Common.Requests;

namespace GroupProject.Application.Topics;

public record CreateTopicRequest(
    string Header,
    string Description,
    CompileOptionsRequest? CompileOptions,
    Guid UserId,
    int SectionId);
