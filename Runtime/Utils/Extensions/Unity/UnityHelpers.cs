using System;
using System.Linq;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public static class UnityTypeHelpers
    {
        private static readonly string[] UnityNamespaces = new string[]
        {
            "UnityEditor",
            "UnityEngine",
        };

        public static bool IsUnityInternal(this MemberInfo type)
        {
            // if (type.Namespace is string ns)
            // {
            //     if (UnityNamespaces.Any(i => ns.StartsWith(i)))
            //     {
            //         return true;
            //     }
            // }

            if (type.DeclaringType?.Namespace is string dns)
            {
                if (UnityNamespaces.Any(i => dns.StartsWith(i)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}