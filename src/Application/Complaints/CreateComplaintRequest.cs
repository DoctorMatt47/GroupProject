using GroupProject.Domain.Enums;

namespace GroupProject.Application.Complaints;

public record CreateComplaintRequest(string Description, ComplaintTarget Target, Guid TargetId);
