using Microsoft.EntityFrameworkCore;

namespace GroupProject.Application.Common.Interfaces;

public interface IAppContext
{
    public DbSet<T> Set<T>() where T : class;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
