using UnityEditor;

namespace UnnamedUiToolkit
{
    public class ArrayEditor : DrawerVisitorTarget
    {
        public ArrayEditor(IUiToolkitValueMember Member)
            : base(Member) { }

        public override NodeVisitor RunNode(NodeVisitor Visitor)
        {
            EditorGUILayout.LabelField(Member.DisplayName, $"Arrays are unsupported for now, use default serializer and complain to inso");

            return Visitor;
        }
    }
}