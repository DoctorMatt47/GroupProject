namespace GroupProject.Application.Complaints;

public record CreateComplaintRequest(string Description, Guid TopicId);
