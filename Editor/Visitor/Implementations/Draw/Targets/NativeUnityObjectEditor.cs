
using System;
using UnityEditor;

namespace UnnamedUiToolkit
{
    public class NativeUnityObjectEditor : DrawerVisitorTarget
    {
        public static bool IsTarget(Type _)
        {
            // Fallback Case For:
            // Transforms, GameObjects, Components, Scripts... 
            return true;
        }

        public NativeUnityObjectEditor(IUiToolkitValueMember Member)
            : base(Member) { }

        public override DrawerVisitor RunNode(DrawerVisitor Visitor)
        {
            var values = Visitor.Values;

            var newVal = EditorGUILayout.ObjectField(Member.DisplayName, obj: (UnityEngine.Object)values.Value, objType: Member.MemberType, allowSceneObjects: true);

            values.Update(newVal);

            return Visitor;
        }
    }
}