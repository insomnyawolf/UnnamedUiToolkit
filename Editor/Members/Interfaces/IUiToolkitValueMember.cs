using System;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public interface IUiToolkitValueMember : IUiToolkitValueMemberInternal<GetDelegate, SetDelegate> { }

    public interface IUiToolkitValueMemberInternal<TGetDelegate, TSetDelegate> : IUiToolkitValueMemberBase
        where TGetDelegate : Delegate
        where TSetDelegate : Delegate
    {
        bool IsRuntimeOnly { get; }
        FieldInfo BackingField { get; }
        Type MemberType { get; }
        public DynamicCall<TGetDelegate> GetExpression { get; }
        public DynamicCall<TSetDelegate> SetExpression { get; }
    }

    public interface IUiToolkitValueMemberBase : IUiToolkitMember
    {
        string RelativePath { get; }
    }
}