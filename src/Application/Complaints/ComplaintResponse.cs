namespace GroupProject.Application.Complaints;

public record ComplaintResponse(
    Guid Id,
    string Description,
    DateTime CreationTime);
