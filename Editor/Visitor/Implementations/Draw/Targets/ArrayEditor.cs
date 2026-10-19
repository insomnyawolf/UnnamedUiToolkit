using System;
using UnityEditor;

namespace UnnamedUiToolkit
{
    public class ArrayEditor : DrawerVisitorTarget
    {
        public static bool IsTarget(Type MemberType)
        {
            if (!MemberType.IsArray)
            {
                return false;
            }

            return true;
        }

        public ArrayEditor(IUiToolkitValueMember Member)
            : base(Member) { }

        public override DrawerVisitor RunNode(DrawerVisitor Visitor)
        {
            EditorGUILayout.LabelField(Member.DisplayName, $"Arrays are unsupported for now, use default serializer and complain to inso");

            return Visitor;
        }
    }
}