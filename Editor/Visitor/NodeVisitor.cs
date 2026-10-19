using System;
using UnityEditor;

namespace UnnamedUiToolkit
{
    public abstract class NodeVisitor<TVisitor> : NodeVisitorBase
        where TVisitor : NodeVisitor<TVisitor>, new ()
    {
        public Func<NodeVisitorBase, bool> UiEventHandler { get; set; }
        public SerializedObject SerializedObject { get; set; }
        public UnityEngine.Object RootTarget { get; set; }

        public object Instance { get; set; }
        public Values Values { get; set; }
        public SerializedPath SerializedPath { get; set; }

        public virtual TVisitor GetChildVisitor()
        {
            var thing = new TVisitor()
            {
                SerializedObject = SerializedObject,
                RootTarget = RootTarget,
                Instance = Values.Value,
                SerializedPath = SerializedPath.GetRelativeChildren(),
                UiEventHandler = UiEventHandler,
            };

            return thing;
        }

        public virtual TOtherVisitor CloneTo<TOtherVisitor>()
            where TOtherVisitor : NodeVisitor<TOtherVisitor>, new()
        {
            var thing = new TOtherVisitor()
            {
                SerializedObject = SerializedObject,
                RootTarget = RootTarget,
                Instance = Values.Value,
                SerializedPath = SerializedPath,
                UiEventHandler = UiEventHandler,
                Values = Values,
            };

            return thing;
        }
    }
}

//UnityEngine.UIElements