using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupProject.Infrastructure.Extensions;

public static class EntityConfigurationExtensions
{
    public static PropertyBuilder<T> HasEnumConversion<T>(this PropertyBuilder<T> builder) where T : struct, Enum
    {
        return builder.HasConversion(status => Enum.GetName(status), str => Enum.Parse<T>(str!));
    }
}
