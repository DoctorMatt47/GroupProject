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

public record TopicHeaderResponse(
    Guid Id,
    string Header,
    DateTime CreationTime,
    int ComplaintCount,
    Guid UserId,
    string UserLogin,
    int SectionId,
    string SectionHeader);

public record TopicByUserIdResponse(
    Guid Id,
    string Header);
