using System;
using System.Collections.Generic;

public class InspectableMemberExtras
{
    public List<Attribute> Attributes { get; set; }

    public void Merge(InspectableMemberExtras newData)
    {
        Attributes.AddRange(newData.Attributes);
    }
}
