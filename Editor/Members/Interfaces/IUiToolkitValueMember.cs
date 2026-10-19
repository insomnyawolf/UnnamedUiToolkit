using System;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public interface IUiToolkitValueMember : IUiToolkitValueMemberBase, IUiToolkitDraw
    {
        bool IsRuntimeOnly { get; }
        FieldInfo BackingField { get; }
        Type MemberType { get; }
        BaseVisitorTarget VisitorTarget { get; }
        public DynamicCall<GetDelegate> GetExpression { get; }
        public DynamicCall<SetDelegate> SetExpression { get; }
    }
}