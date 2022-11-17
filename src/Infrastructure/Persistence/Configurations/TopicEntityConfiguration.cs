using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupProject.Infrastructure.Persistence.Configurations;

public class TopicEntityConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasOne(t => t.User).WithMany(u => u.Topics).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(t => t.Section).WithMany(s => s.Topics).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(t => t.Complaints).WithOne(c => c.Topic).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(t => t.Commentaries).WithOne(c => c.Topic).OnDelete(DeleteBehavior.Cascade);
        builder.OwnsOne(t => t.CompileOptions);
    }
}
