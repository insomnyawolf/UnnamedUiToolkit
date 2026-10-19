using System;
using System.Collections.Generic;

namespace UnnamedUiToolkit
{
    public interface IUiToolkitElement : IUiToolkitBase
    {
        string DisplayName { get; }
        List<Attribute> Attributes { get; }
        public bool IsSkip { get; }
        public bool IsHide { get; }
    }
}