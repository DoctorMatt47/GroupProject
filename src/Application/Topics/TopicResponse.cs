using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Topics;

public record TopicResponse(
    Guid Id,
    string Header,
    string Description,
    CompileOptionsResponse? CompileOptions,
    DateTime CreationTime,
    int ComplaintCount,
    Guid UserId,
    string UserLogin,
    int SectionId,
    string SectionHeader);
