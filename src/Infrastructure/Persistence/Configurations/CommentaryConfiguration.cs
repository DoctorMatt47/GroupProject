using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupProject.Infrastructure.Persistence.Configurations;

public class CommentaryConfiguration : IEntityTypeConfiguration<Commentary>
{
    public void Configure(EntityTypeBuilder<Commentary> builder)
    {
        builder.HasOne(c => c.User).WithMany(u => u.Commentaries);
        builder.HasOne(c => c.Topic).WithMany(t => t.Commentaries);
        builder.HasMany(c => c.Complaints).WithOne(c => c.Commentary);
        builder.OwnsOne(c => c.CompileOptions);
    }
}
