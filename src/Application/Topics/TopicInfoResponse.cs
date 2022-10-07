namespace GroupProject.Application.Topics;

public record TopicInfoResponse(
    Guid Id,
    string Header,
    Guid UserId,
    string UserLogin);