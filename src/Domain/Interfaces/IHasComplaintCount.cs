namespace GroupProject.Domain.Interfaces;

public interface IHasComplaintCount
{
    void IncrementComplaintCount();
    void DecrementComplaintCount();
}
