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
    public DbSet<Section> Sections { get; set; } = null!;
    public DbSet<Configuration> Configurations { get; set; } = null!;
    public DbSet<ForbiddenPhrase> ForbiddenPhrases { get; set; } = null!;
    public DbSet<VerificationRequiredPhrase> VerificationRequiredPhrases { get; set; } = null!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await HandleTopics(cancellationToken);
        await HandleComplaints(cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    private async Task HandleComplaints(CancellationToken cancellationToken)
    {
        var added = GetEntitiesByState<Complaint>(EntityState.Added);
        var deleted = GetEntitiesByState<Complaint>(EntityState.Deleted);

        await ExecuteUpdateCountAsync(
            added.Where(c => c.CommentaryId is not null),
            deleted.Where(c => c.CommentaryId is not null),
            nameof(Commentaries),
            nameof(Commentary.ComplaintCount),
            t => t.CommentaryId,
            cancellationToken);

        await ExecuteUpdateCountAsync(
            added.Where(c => c.TopicId is not null),
            deleted.Where(c => c.TopicId is not null),
            nameof(Topics),
            nameof(Topic.ComplaintCount),
            t => t.TopicId,
            cancellationToken);
    }

    private async Task HandleTopics(CancellationToken cancellationToken)
    {
        var added = GetEntitiesByState<Topic>(EntityState.Added);
        var deleted = GetEntitiesByState<Topic>(EntityState.Deleted);

        await ExecuteUpdateCountAsync(
            added,
            deleted,
            nameof(Sections),
            nameof(Section.TopicCount),
            t => t.SectionId,
            cancellationToken);
    }

    private async Task ExecuteUpdateCountAsync<T, TKey>(
        IEnumerable<T> added,
        IEnumerable<T> deleted,
        string tableName,
        string countColumnName,
        Func<T, TKey> groupBy,
        CancellationToken cancellationToken)
    {
        var groupGroups = GetCountsGroup(added, deleted, groupBy);

        foreach (var countsGroup in groupGroups)
        {
            var count = countsGroup.Added - countsGroup.Deleted;
            await Database.ExecuteSqlRawAsync(
                UpdateCountScript(
                    tableName,
                    countColumnName,
                    count),
                cancellationToken);
        }
    }

    private static IEnumerable<(int Added, int Deleted)>
        GetCountsGroup<T, TKey>(
            IEnumerable<T> added,
            IEnumerable<T> deleted,
            Func<T, TKey> groupBy)
    {
        var addedGroup = added.GroupBy(groupBy).Select(t => new {State = EntityState.Added, Group = t});
        var deletedGroup = deleted.GroupBy(groupBy).Select(t => new {State = EntityState.Deleted, Group = t});

        var groupGroups = addedGroup
            .Concat(deletedGroup)
            .GroupBy(g => g.Group.Key)
            .Select(g =>
            (
                Added: g.Where(t => t.State == EntityState.Added).Select(t => t.Group).Count(),
                Deleted: g.Where(t => t.State == EntityState.Deleted).Select(t => t.Group).Count()
            ))
            .ToList();

        return groupGroups;
    }

    private IReadOnlyCollection<T> GetEntitiesByState<T>(EntityState state) =>
        ChangeTracker.Entries()
            .Where(e => e.State == state)
            .Where(e => e.Metadata.ClrType == typeof(T))
            .Select(e => e.Entity)
            .Cast<T>()
            .ToList();

    private static string UpdateCountScript(
        string tableName,
        string columnName,
        int count) =>
        $"update {tableName} set {columnName} = {columnName} + {count}";
}
