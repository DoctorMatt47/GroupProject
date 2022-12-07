using System.Reflection;

namespace GroupProject.Application.IntegrationTests.Common.Utils;

public class ReflectionUtil
{
    public static PropertyInfo? GetPrivateSetProperty<T>(string property) =>
        typeof(T).GetProperty(property, BindingFlags.Instance | BindingFlags.Public);
}

