
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetClassesOfTypeInItsAssembly<TType>()
        {
            var TargetType = typeof(TType);

            var Assembly = TargetType.Assembly;

            var allTypes = Assembly.GetTypes();

            var filtered = allTypes.Where(Filter);

            return filtered;

            bool Filter(Type Type)
            {
                if (Type.IsAbstract)
                {
                    return false;
                }

                if (!Type.IsClass)
                {
                    return false;
                }

                if (!Type.IsPublic)
                {
                    return false;
                }

                if (!Type.IsSubclassOf(TargetType))
                {
                    return false;
                }

                return true;
            }
        }

    }
}