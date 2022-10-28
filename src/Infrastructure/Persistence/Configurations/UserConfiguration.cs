using GroupProject.Domain.Entities;
using GroupProject.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupProject.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(u => u.Topics).WithOne(c => c.User);
        builder.HasMany(u => u.Commentaries).WithOne(c => c.User);
        builder.Property(u => u.Role).HasEnumConversion();
        builder.Property(u => u.Status).HasEnumConversion();
    }
}
