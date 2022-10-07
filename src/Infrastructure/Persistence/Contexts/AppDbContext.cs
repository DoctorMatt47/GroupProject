using System.Reflection;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroupProject.Infrastructure.Persistence.Contexts;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Topic> Topics { get; set; } = null!;
    public DbSet<Complaint> Complaints { get; set; } = null!;
    public DbSet<Commentary> Commentaries { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
