using System;
using UnityEditor;

/*
TO DO 

Save not shown types with attr

Modify shown editor with attribute

 */
namespace UnnamedUiToolkit
{
    [UnnamedUiToolkit]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class UnnamedUiToolkitEditor : Editor
    {
        protected Action OnGuiHandler;
        protected TreeHandler InspectorTreeHandler;
        protected DrawerVisitor DrawerVisitor;
        protected UpdateVisitor UpdateVisitor;

        protected void OnEnable()
        {
            if (target == null) return;

            var TargetType = target.GetType();

            InspectorTreeHandler = new TreeHandler(TargetType);

            if (!InspectorTreeHandler.IsValidRootTarget)
            {
                // If has none of our attributes we don't care about it
                OnGuiHandler = base.OnInspectorGUI;

                return;
            }

            InspectorTreeHandler.Initialize();

            UpdateVisitor = new UpdateVisitor()
            {
                RootTarget = target,
                SerializedObject = serializedObject,
                Instance = target,
                SerializedPath = new SerializedPath(),
            };

            DrawerVisitor = new DrawerVisitor()
            {
                RootTarget = target,
                SerializedObject = serializedObject,
                Instance = target,
                SerializedPath = new SerializedPath(),
            };

            DrawerVisitor.UiEventHandler += UpdateCascade;
            
            OnGuiHandler = CustomInspectorGui;
        }

        protected void CustomInspectorGui()
        {
            using var ChangeCheck = new EditorGUI.ChangeCheckScope();

            InspectorTreeHandler.VisitNodes(DrawerVisitor);

            if (ChangeCheck.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        public override void OnInspectorGUI()
        {
            OnGuiHandler();
        }

        protected void UpdateCascade(NodeVisitor NodeVisitor, UiEventType UiEventType)
        {
            if (UiEventType != UiEventType.Update)
            {
                return;
            }

            InspectorTreeHandler.VisitNodes(UpdateVisitor);
        }
    }
}