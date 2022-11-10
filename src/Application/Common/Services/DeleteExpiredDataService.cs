using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Common.Services;

public class DeleteExpiredDataService : IDeleteExpiredDataService
{
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<DeleteExpiredDataService> _logger;

    public DeleteExpiredDataService(IAppDbContext dbContext, ILogger<DeleteExpiredDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task DeleteExpiredComplaints(CancellationToken cancellationToken)
    {
        var complaints = await _dbContext.Set<Complaint>()
            .Where(c => c.ExpirationTime < DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        _dbContext.Set<Complaint>().RemoveRange(complaints);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
