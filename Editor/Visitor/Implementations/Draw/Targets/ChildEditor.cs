using UnityEditor;

namespace UnnamedUiToolkit
{
    public class ChildEditor : ChildVisitorTarget<DrawerVisitor>
    {
        public ChildEditor(IUiToolkitValueMember Member)
             : base(Member) { }

        protected bool IsFoldoutOpen { get; set; } = true;

        public override NodeVisitor RunNode(NodeVisitor Visitor)
        {
            // Create a foldout header for the nested object
            IsFoldoutOpen = EditorGUILayout.Foldout(foldout: IsFoldoutOpen, content: Member.DisplayName, toggleOnLabelClick: true);

            if (!IsFoldoutOpen) return Visitor;

            using var indent = new EditorGUI.IndentLevelScope();

            var res = base.RunNode(Visitor);

            return res;
        }
    }
}