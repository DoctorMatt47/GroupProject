using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupProject.Infrastructure.Persistence.Configurations;

public class CommentaryConfiguration : IEntityTypeConfiguration<Commentary>
{
    public void Configure(EntityTypeBuilder<Commentary> builder)
    {
        builder.HasOne(c => c.User).WithMany(u => u.Commentaries);
    }
}