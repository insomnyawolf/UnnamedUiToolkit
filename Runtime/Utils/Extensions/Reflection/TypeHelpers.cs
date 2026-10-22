using System;

namespace UnnamedUiToolkit
{
    public static class TypeHelpers
    {
        public static bool IsBasic(this Type type)
        {
            if (type.IsPrimitive)
            {
                return true;
            }

            if (type.IsEnum)
            {
                return true;
            }

            if (type == typeof(string))
            {
                return true;
            }

            if (type == typeof(decimal))
            {
                return true;
            }

            return false;
        }
    }
}