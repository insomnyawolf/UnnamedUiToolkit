using System;

namespace UnnamedUiToolkit
{
    public interface IOnChangeMember : IUiToolkitMember
    {
        public DynamicCall<OnChange> OnChangeExpr { get; }
    }
}