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

            OnGuiHandler = CustomInspectorGui;
        }

        protected void CustomInspectorGui()
        {
            var visitor = new DrawerVisitor()
            {
                RootTarget = target,
                SerializedObject = serializedObject,
                Instance = target,
                SerializedPath = new SerializedPath(),
            };

            InspectorTreeHandler.VisitNodes(visitor);
        }

        public override void OnInspectorGUI()
        {
            OnGuiHandler();
        }
    }
}