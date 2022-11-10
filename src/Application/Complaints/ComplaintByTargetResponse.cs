namespace GroupProject.Application.Complaints;

public record ComplaintByTargetResponse(
    Guid Id,
    string Description,
    Guid UserId,
    DateTime CreationTime);
