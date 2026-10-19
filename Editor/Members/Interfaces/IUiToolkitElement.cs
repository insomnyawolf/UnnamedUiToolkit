using System;
using System.Collections.Generic;

namespace UnnamedUiToolkit
{
    public interface IUiToolkitElement : IUiToolkitBase
    {
#warning could be only on serialized members but if it's here it reduces the edge cases
        /// <summary>
        /// In the same format as unity SerializedProperty paths
        /// </summary>
        string DisplayName { get; }
        List<Attribute> Attributes { get; }

        public bool IsSkip { get; }
        public bool IsHide { get; }

        //Action<NodeVisitor> CachedDrawer { get; }
    }
}