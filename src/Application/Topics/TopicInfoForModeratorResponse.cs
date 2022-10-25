namespace GroupProject.Application.Topics;

public record TopicInfoForModeratorResponse(
    Guid Id,
    string Header,
    Guid UserId,
    string UserLogin,
    DateTime CreationTime,
    int ComplaintCount,
    int SectionId,
    string SectionHeader);
