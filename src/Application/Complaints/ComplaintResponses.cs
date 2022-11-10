namespace GroupProject.Application.Complaints;

public record ComplaintResponse(
    Guid Id,
    string Description,
    Guid UserId,
    DateTime CreationTime,
    Guid TargetId,
    string Target);

public record ComplaintByTargetResponse(
    Guid Id,
    string Description,
    Guid UserId,
    DateTime CreationTime);
