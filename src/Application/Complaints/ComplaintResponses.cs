namespace GroupProject.Application.Complaints;

public record ComplaintResponse(
    Guid Id,
    string Description,
    Guid UserId,
    DateTime CreationTime,
    DateTime ExpirationTime,
    string Target,
    Guid TargetId);

public record ComplaintByTargetResponse(
    Guid Id,
    string Description,
    Guid UserId,
    DateTime CreationTime,
    DateTime ExpirationTime);
