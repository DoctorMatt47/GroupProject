namespace GroupProject.Application.Topics;

public record TopicInfoResponse(
    Guid Id,
    string Header,
    DateTime CreationTime,
    int ComplaintCount,
    Guid UserId,
    string UserLogin,
    int SectionId,
    string SectionHeader);
