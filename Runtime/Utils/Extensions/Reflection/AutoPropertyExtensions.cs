using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public static class AutoPropertyExtensions
{
    // $"<{prop.Name}>k__BackingField";

    private const string Prefix = "<";
    private const string Suffix = ">k__BackingField";
    private static readonly Regex AutoPropertyRegex = new($"{Prefix}(.+?){Suffix}", RegexOptions.Singleline | RegexOptions.Compiled);

    private static string GetBackingFieldName(string propertyName) => $"{Prefix}{propertyName}{Suffix}";

    private static string GetAutoPropertyName(string fieldName)
    {
        var match = AutoPropertyRegex.Match(fieldName);

        if (!match.Success) return null;

        return match.Groups[1].Value;
    }

    public static bool IsAnAutoProperty(this PropertyInfo propertyInfo) => propertyInfo.GetBackingField() != null;

    public static bool IsBackingFieldOfAnAutoProperty(this FieldInfo propertyInfo) => propertyInfo.GetAutoProperty() != null;

    public static FieldInfo GetBackingField(this PropertyInfo propertyInfo)
    {
        if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));

        if (!propertyInfo.CanRead || !propertyInfo.GetGetMethod(nonPublic: true).IsDefined(typeof(CompilerGeneratedAttribute), inherit: true))
            return null;

        var backingFieldName = GetBackingFieldName(propertyInfo.Name);

        var backingField = propertyInfo.DeclaringType?.GetField(backingFieldName, BindingFlags.Instance | BindingFlags.NonPublic);

        if (backingField is null) return null;

        if (!backingField.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true)) return null;

        return backingField;
    }

    public static PropertyInfo GetAutoProperty(this FieldInfo fieldInfo)
    {
        if (fieldInfo is null) throw new ArgumentNullException(nameof(fieldInfo));

        if (!fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true)) return null;

        var autoPropertyName = GetAutoPropertyName(fieldInfo.Name);

        if (autoPropertyName is null) return null;

        var autoProperty = fieldInfo.DeclaringType?.GetProperty(autoPropertyName);

        if (autoProperty is null) return null;

        if (!autoProperty.CanRead || !autoProperty.GetGetMethod(nonPublic: true).IsDefined(typeof(CompilerGeneratedAttribute), inherit: true))
            return null;

        return autoProperty;
    }
}