using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace UnnamedUiToolkit
{
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

        ////[Experimental]
        // private static FieldInfo GetExactBackingFieldFromIL(PropertyInfo property)
        // {
        //     var getter = property.GetGetMethod(true);
        //     if (getter == null) return null;

        //     var body = getter.GetMethodBody();
        //     if (body == null) return null;

        //     byte[] ilBytes = body.GetILAsByteArray();

        //     // scan for ldfld (OpCode value 0x7B)
        //     for (int i = 0; i < ilBytes.Length - 4; i++)
        //     {
        //         // 0xFE, 0x7B is ldfld in some contexts, but standard inline token ldfld starts with 0x7B 
        //         // Note: IL instruction tokens are 4 bytes following the opcode.
        //         ushort opCode = (i > 0 && ilBytes[i - 1] == 0xFE)
        //             ? (ushort)((ilBytes[i - 1] << 8) | ilBytes[i])
        //             : (ushort)ilBytes[i];

        //         // LDFLD opcode check (0x7B)
        //         if (ilBytes[i] == 0x7B)
        //         {
        //             int metadataToken = BitConverter.ToInt32(ilBytes, i + 1);
        //             try
        //             {
        //                 var module = getter.Module;
        //                 var member = module.ResolveMember(metadataToken);
        //                 if (member is FieldInfo field)
        //                 {
        //                     return field;
        //                 }
        //             }
        //             catch
        //             {
        //                 // Token resolution can occasionally fail depending on context, ignore and keep scanning
        //             }
        //         }
        //     }

        //     return null;
        // }
    }
}