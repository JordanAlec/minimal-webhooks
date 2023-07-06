namespace MinimalWebHooks.Core.Extensions;

public static class ObjectExtensions
{
    public static string? GetEntityTypeName(this object data) => data.GetType().GetEntityTypeName();

    public static string? GetEntityTypeName(this Type type) => type.FullName;
}