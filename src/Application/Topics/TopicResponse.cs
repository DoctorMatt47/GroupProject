namespace GroupProject.Application.Topics;

public record TopicResponse(
    Guid Id,
    string Header,
    string Description,
    string? Code,
    Guid UserId,
    string UserLogin);