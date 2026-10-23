
using System;

namespace UnnamedUiToolkit
{
    public static class EditorTypeHelper
    {
        public static bool IsArray(this Type MemberType)
        {
            if (!MemberType.IsArray)
            {
                return false;
            }

            return true;
        }

        public static bool IsEnum(this Type MemberType)
        {
            if (!MemberType.IsEnum) return false;

            return true;
        }

        public static bool IsPrimitive(this Type MemberType, out InspectorBinding binding)
        {
            binding = null;

            if (MemberType.IsEnum)
            {
                return false;
            }

            if (MemberType.IsArray)
            {
                return false;
            }

            if (MemberType.IsUnityInternal())
            {
                return false;
            }

            if (!InspectorBindingCache.PropertyDrawersCache.TryGetValue(MemberType, out binding))
            {
                return false;
            }

            return true;
        }

        public static bool IsChild(this Type MemberType)
        {
#warning This check is not good enough

            // Structs?
            // This is pain
            if (!MemberType.IsClass)
            {
                return false;
            }

            if (MemberType.IsArray)
            {
                return false;
            }

            if (MemberType.IsBasic())
            {
                return false;
            }

            if (MemberType.IsUnityObject())
            {
                return false;
            }

            if (MemberType.IsUnityInternal())
            {
                return false;
            }

            return true;
        }

        public static bool IsUnityObject(this Type MemberType)
        {
            // Fallback Case For:
            // Transforms, GameObjects, Components, Scripts... 
            var isUnityObject = typeof(UnityEngine.Object).IsAssignableFrom(MemberType);

            return isUnityObject;
        }
    }
}