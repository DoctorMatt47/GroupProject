namespace GroupProject.Domain.Interfaces;

public interface IHasComplaintCount
{
    int ComplaintCount { get; }
    void IncrementComplaintCount();
    void DecrementComplaintCount();
}
