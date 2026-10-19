using System;
using UnityEngine;

namespace UnnamedUiToolkit
{
    public abstract class BaseVisitorTarget<TVisitor>
        where TVisitor : NodeVisitor<TVisitor>, new()
    {
        public static readonly GUILayoutOption[] EmptyGuiLayoutOptions = Array.Empty<GUILayoutOption>();

        public IUiToolkitValueMember Member { get; }

        public BaseVisitorTarget(IUiToolkitValueMember Member)
        {
            this.Member = Member;
        }

        public abstract TVisitor RunNode(TVisitor value);
    }
}
