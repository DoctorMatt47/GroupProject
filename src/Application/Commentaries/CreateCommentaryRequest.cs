namespace GroupProject.Application.Commentaries;

public record CreateCommentaryRequest(
    string Description,
    string? Code,
    Guid TopicId);
