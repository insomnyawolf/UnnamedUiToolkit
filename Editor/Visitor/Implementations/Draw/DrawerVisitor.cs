using UnityEditor;
using UnityEngine;

namespace UnnamedUiToolkit
{
    public class DrawerVisitor : NodeVisitor<DrawerVisitor>
    {
        public override void Visit(IUiToolkitBase Target)
        {
            if (Target is not IUiToolkitDraw drawable)
            {
                return;
            }

            if (Target is IUiToolkitValueMember ValueMember)
            {
                SerializedPath.SubPath = ValueMember.RelativePath;

                ProcessValueMember(drawable, ValueMember);

                return;
            }

            if (Target is IUiToolkitValueMemberBase ValueMemberBase)
            {
                SerializedPath.SubPath = ValueMemberBase.RelativePath;

                drawable.Draw(this);

                return;
            }

            drawable.Draw(this);
        }

        public void ProcessValueMember(IUiToolkitDraw drawable, IUiToolkitValueMember ValueMember)
        {
            Values = new Values();

            var V0 = ValueMember.GetExpression.Compiled(Instance);

            Values.Update(V0);

            var SerializedProperty = GetPropertyIfSerializable();

            if (SerializedProperty?.isArray == true)
            {
                // im lazy, sorry
                SerializedProperty = null;
            }

            if (SerializedProperty != null)
            {
                Values.Update(SerializedProperty.boxedValue);
            }

            drawable.Draw(this);

            if (!Values.HasChanged())
            {
                return;
            }

            var newValue = Values.Value;

            ValueMember.SetExpression.Compiled(Instance, newValue);

            if (SerializedProperty == null)
            {
                return;
            }

            Undo.RecordObject(RootTarget, $"Changed {drawable.DisplayName}");

            SerializedProperty.boxedValue = newValue;

            SerializedProperty.serializedObject.ApplyModifiedProperties();

            // Not what we need
            //sp.serializedObject.Update();

            EditorUtility.SetDirty(RootTarget);
        }

        public SerializedProperty GetPropertyIfSerializable()
        {
            //if (Metadata.IsRuntimeOnly) return null;
            //if (Metadata.MemberType.IsArray) return null;

            var fullPath = SerializedPath.FullPath;

            var sp = SerializedObject.FindProperty(fullPath);

            return sp;
        }
    }
}