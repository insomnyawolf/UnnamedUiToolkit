using UnityEditor;

namespace UnnamedUiToolkit
{
    public class NativeUnityObjectEditor : DrawerVisitorTarget
    {
        public NativeUnityObjectEditor(IUiToolkitValueMember Member)
            : base(Member) { }

        public override NodeVisitor RunNode(NodeVisitor Visitor)
        {
            var values = Visitor.Values;

            var newVal = EditorGUILayout.ObjectField(Member.DisplayName, obj: (UnityEngine.Object)values.Value, objType: Member.MemberType, allowSceneObjects: true);

            values.Update(newVal);

            return Visitor;
        }
    }
}