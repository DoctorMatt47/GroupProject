using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupProject.Infrastructure.Persistence.Configurations;

public class SectionEntityConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasMany(s => s.Topics).WithOne(t => t.Section).OnDelete(DeleteBehavior.Cascade);
    }
}
