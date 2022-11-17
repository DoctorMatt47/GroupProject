using GroupProject.Domain.Entities;
using GroupProject.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupProject.Infrastructure.Persistence.Configurations;

public class ComplaintEntityConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasOne(c => c.Topic).WithMany(t => t.Complaints).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(c => c.Commentary).WithMany(t => t.Complaints).OnDelete(DeleteBehavior.Cascade);
        builder.Property(c => c.Target).HasEnumConversion();
    }
}
