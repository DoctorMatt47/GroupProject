using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroupProject.Infrastructure.Persistence.Contexts;

public sealed class AppContext : DbContext, IAppContext
{
    public AppContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Topic> Topics { get; set; } = null!;
    public DbSet<Complaint> Complaints { get; set; } = null!;
    public DbSet<Commentary> Commentaries { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
