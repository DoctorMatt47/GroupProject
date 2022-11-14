namespace GroupProject.Application.Common.Services;

public interface IDeleteExpiredDataService
{
    Task DeleteExpiredComplaints(CancellationToken cancellationToken);
}
