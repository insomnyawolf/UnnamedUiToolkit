using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class AttributeExtensions
{
    public static List<Attribute> GetAllAttributres(this MemberInfo MemberInfo)
    {
        var attrs = MemberInfo.GetCustomAttributes(inherit: true).Cast<Attribute>().ToList();

        return attrs;
    }

    public static bool TryFindAttribute<TAttribute>(this List<Attribute> Attributes, out TAttribute Attribute)
        where TAttribute : Attribute
    {
        for (int i = 0; i < Attributes.Count; i++)
        {
            var c = Attributes[i];

            if (c is TAttribute Attr)
            {
                Attribute = Attr;

                return true;
            }
        }

        Attribute = null;

        return false;
    }
}
