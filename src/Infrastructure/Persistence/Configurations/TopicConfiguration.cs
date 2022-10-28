using GroupProject.Domain.Entities;
using GroupProject.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupProject.Infrastructure.Persistence.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasOne(t => t.User).WithMany(u => u.Topics);
        builder.HasOne(t => t.Section).WithMany(s => s.Topics);
        builder.HasMany(t => t.Complaints).WithOne(c => c.Topic);
        builder.Property(t => t.Status).HasEnumConversion();
    }
}
