using System;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public interface IUiToolkitMember : IUiToolkitElement
    {
        string ReflectedName { get; }
        MemberInfo MemberInfo { get; }
    }
}