
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UnnamedUiToolkit
{
    public static class GenericReflectionHelpers
    {
        public const BindingFlags ReflectionFlags =
        BindingFlags.Instance
        |
        BindingFlags.Public
        |
        BindingFlags.NonPublic
        |
        BindingFlags.FlattenHierarchy
        ;

        public static bool TryGetInstance(this Delegate action, out object Target, out Type TargetType, out bool IsClosure)
        {
            IsClosure = false;
            TargetType = null;
            Target = action?.Target;

            if (Target is null)
            {
                return false;
            }

            TargetType = Target.GetType();

            IsClosure = TargetType.GetCustomAttribute<CompilerGeneratedAttribute>() is not null;

            return true;
        }
    }
}