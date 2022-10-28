using GroupProject.Domain.ValueObjects;

namespace GroupProject.Application.Topics;

public record TopicResponse(
    Guid Id,
    string Header,
    string Description,
    CompileOptions CompileOptions,
    DateTime CreationTime,
    int ComplaintCount,
    Guid UserId,
    string UserLogin,
    int SectionId,
    string SectionHeader);
