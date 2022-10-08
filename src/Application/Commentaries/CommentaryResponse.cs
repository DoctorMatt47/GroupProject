namespace GroupProject.Application.Commentaries;

public record CommentaryResponse(
    Guid Id,
    string Description,
    string? Code,
    Guid UserId,
    Guid TopicId,
    DateTime CreationTime);
