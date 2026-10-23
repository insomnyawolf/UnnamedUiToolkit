using UnityEditor;

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
                var savedVal = SerializedProperty.boxedValue;

                Values.Update(savedVal);
            }

            ValueMember.Draw(this);
            if (!Values.HasChanged())
            {

                // WHYYYYYYYYYYYYYYYYYYYYY
                // Without the:
                // !ValueMember.MemberType.IsChild()
                // Serialized fields fail to update on events ....
                // I'm so confused ...
                // if (!ValueMember.MemberType.IsChild())
                // {
                //     return;
                // }

                // Nvm using "Undo.RegisterCompleteObjectUndo" 
                // instead of "Undo.RecordObject"
                // fixed it :)
                // Finally \:D/

                return;
            }

            Undo.RegisterCompleteObjectUndo(RootTarget, $"Changed {ValueMember.DisplayName}");

            var newValue = Values.Value;

            ValueMember.SetExpression.Compiled(Instance, newValue);

            if (SerializedProperty != null)
            {
                SerializedProperty.boxedValue = newValue;
            }

            if (ValueMember is IOnChangeMember onChange)
            {
                if (onChange.OnChangeExpr != null)
                {
                    TriggerUiEvent(UiEventType.Update);
                }
            }

            SerializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(RootTarget);
        }
    }
}