using GroupProject.Domain.ValueObjects;

namespace GroupProject.Application.Topics;

public record CreateTopicRequest(
    string Header,
    string Description,
    CompileOptions? CompileOptions,
    Guid UserId,
    int SectionId);
