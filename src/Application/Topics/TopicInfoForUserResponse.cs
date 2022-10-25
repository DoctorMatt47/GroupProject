namespace GroupProject.Application.Topics;

public record TopicInfoForUserResponse(
    Guid Id,
    string Header,
    Guid UserId,
    string UserLogin,
    DateTime CreationTime,
    int SectionId,
    string SectionHeader);
