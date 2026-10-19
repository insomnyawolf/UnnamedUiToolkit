using System;
using System.Reflection;
using UnityEditor;

namespace UnnamedUiToolkit
{
    public class DefaultSerializedUiToolkitElement : UiToolkitElement, IUiToolkitValueMemberBase, IUiToolkitDraw
    {
        public bool IncludeChildren { get; set; }

        public string ReflectedName { get; protected set; }
        public MemberInfo MemberInfo { get; protected set; }

        public Type MemberType { get; protected set; }

        public FieldInfo BackingField { get; protected set; }
        public string RelativePath => BackingField.Name;

        public DefaultSerializedUiToolkitElement(MemberInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
            : base(InspectableMemberExtras)
        {
            this.MemberInfo = MemberInfo;

            ReflectedName = MemberInfo.Name;

            if (MemberInfo is PropertyInfo pi)
            {
                MemberType = pi.PropertyType;
                BackingField = pi.GetBackingField();
            }
            else if (MemberInfo is FieldInfo fi)
            {
                MemberType = fi.FieldType;
                BackingField = fi;
            }
        }

        protected override void ProcessAttribute(Attribute Attribute)
        {
            if (Attribute is DefaultSerializerAttribute ds)
            {
                IncludeChildren = ds.IncludeChildren;
                return;
            }

            base.ProcessAttribute(Attribute);
        }

        public DrawerVisitor Draw(DrawerVisitor Visitor)
        {
            var SerializedProperty = Visitor.GetPropertyIfSerializable();

            if (SerializedProperty is null)
            {
                return Visitor;
            }

            using var ChangeCheck = new EditorGUI.ChangeCheckScope();

            var newVal = EditorGUILayout.PropertyField(property: SerializedProperty, includeChildren: true);

            if (ChangeCheck.changed)
            {
                SerializedProperty.serializedObject.ApplyModifiedProperties();
            }

            return Visitor;
        }
    }
}