using System;
using UnityEditor;

namespace UnnamedUiToolkit
{
    public abstract class NodeVisitor : NodeVisitorBase
    {
        public event Action<NodeVisitor, UiEventType> UiEventHandler;
        public SerializedObject SerializedObject { get; set; }
        public UnityEngine.Object RootTarget { get; set; }

        public object Instance { get; set; }
        public Values Values { get; set; }
        public SerializedPath SerializedPath { get; set; }

        public virtual TVisitor GetChildVisitor<TVisitor>()
            where TVisitor : NodeVisitor, new()
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
            where TOtherVisitor : NodeVisitor, new()
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

        public SerializedProperty GetPropertyIfSerializable()
        {
            //if (Metadata.IsRuntimeOnly) return null;
            //if (Metadata.MemberType.IsArray) return null;

            var fullPath = SerializedPath.FullPath;

            var sp = SerializedObject.FindProperty(fullPath);

            return sp;
        }

        public void TriggerUiEvent(UiEventType UiEventType)
        {
            UiEventHandler(this, UiEventType);
        }
    }
}