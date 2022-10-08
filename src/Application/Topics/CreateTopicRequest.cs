namespace GroupProject.Application.Topics;

public record CreateTopicRequest(
    string Header,
    string Description,
    string? Code,
    Guid UserId);
