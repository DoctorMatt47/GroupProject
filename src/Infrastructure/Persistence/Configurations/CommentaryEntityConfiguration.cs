using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupProject.Infrastructure.Persistence.Configurations;

public class CommentaryEntityConfiguration : IEntityTypeConfiguration<Commentary>
{
    public void Configure(EntityTypeBuilder<Commentary> builder)
    {
        builder.HasOne(c => c.User).WithMany(u => u.Commentaries).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(c => c.Topic).WithMany(t => t.Commentaries).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.Complaints).WithOne(c => c.Commentary).OnDelete(DeleteBehavior.Cascade);
        builder.OwnsOne(c => c.CompileOptions);
    }
}
