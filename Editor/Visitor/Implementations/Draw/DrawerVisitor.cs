using UnityEditor;
using UnityEngine;

namespace UnnamedUiToolkit
{
    public class DrawerVisitor : NodeVisitor
    {
        public override void Visit(IUiToolkitBase Target)
        {
            if (Target is IUiToolkitValueMemberBase ValueMemberBase)
            {
                SerializedPath.SubPath = ValueMemberBase.RelativePath;
            }

            if (Target is IUiToolkitValueMember ValueMember)
            {
                ProcessValueMember(ValueMember);

                return;
            }

            if (Target is IUiToolkitDraw IUiToolkitDraw)
            {
                IUiToolkitDraw.Draw(this);

                return;
            }

        }

        public void ProcessValueMember(IUiToolkitValueMember ValueMember)
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

            ValueMember.Draw(this);

            if (!Values.HasChanged())
            {
                return;
            }

            var newValue = Values.Value;

            ValueMember.SetExpression.Compiled(Instance, newValue);

            if (ValueMember is IOnChangeMember onChange)
            {
                if (onChange.OnChangeExpr != null)
                {
                    TriggerUiEvent(UiEventType.Update);
                }
            }

            if (SerializedProperty == null)
            {
                return;
            }

            Undo.RecordObject(RootTarget, $"Changed {ValueMember.DisplayName}");

            SerializedProperty.boxedValue = newValue;

            //SerializedProperty.serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(RootTarget);
        }
    }
}