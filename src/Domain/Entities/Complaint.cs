namespace GroupProject.Domain.Entities;

public class Complaint
{
    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    protected Complaint()
    {
    }

    public Complaint(string description, Guid questionId)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
        Description = description;
        QuestionId = questionId;
    }

    public Guid Id { get; protected set; }
    public DateTime CreationTime { get; protected set; }
    public string Description { get; protected set; } = null!;

    public Guid QuestionId { get; protected set; }
    public Topic Topic { get; protected set; } = null!;
}
