using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GroupProject.Application.Common.Interfaces;

public interface IAppDbContext
{
    public DatabaseFacade Database { get; }
    public DbSet<T> Set<T>() where T : class;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
