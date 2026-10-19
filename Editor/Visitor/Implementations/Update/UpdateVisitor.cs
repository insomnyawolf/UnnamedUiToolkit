using UnityEditor;

namespace UnnamedUiToolkit
{
    public class UpdateVisitor : NodeVisitor
    {
        public UpdateVisitor() { }

        public override void Visit(IUiToolkitBase Target)
        {
            if (Target is not IUiToolkitValueMember ValueMember)
            {
                // No value, nothing to update
                return;
            }

            SerializedPath.SubPath = ValueMember.RelativePath;

            if (GetPropertyIfSerializable() is not SerializedProperty SerializedProperty)
            {
                // Not serialized fields doesn't need this fix
                // All members updated instantly in the code and interface
                // But those updates doesn't automatically carry to serialized properties
                // That is why this exist
                return;
            }

            if (ValueMember.MemberType.IsArray())
            {
                return;
            }

            if (ValueMember.MemberType.IsChild())
            {
                Values = new Values();

                var V0 = ValueMember.GetExpression.Compiled(Instance);

                Values.Update(V0);

                var cu = new ChildUpdater(ValueMember);

                cu.RunNode(this);
                // Unsure if i need this return;
                return;
            }

            TryPersistOutdatedValue(ValueMember, SerializedProperty);
        }

        protected void TryPersistOutdatedValue(IUiToolkitValueMember ValueMember, SerializedProperty SerializedProperty)
        {
            Values = new Values();

            var currentSerialized = SerializedProperty.boxedValue;

            Values.Update(currentSerialized);

            var currentCode = ValueMember.GetExpression.Compiled(Instance);

            Values.Update(currentCode);

            if (!Values.HasChanged())
            {
                // No change
                return;
            }

            var newVal = Values.Value;

            // Looks redundant but this triggers the OnChange and other appended callbacks automatically
            // I'm lazy to do it better for now
            ValueMember.SetExpression.Compiled(Instance, newVal);

            // Should be a reaction to another event thefore this should not be needed
            //Undo.RecordObject(RootTarget, $"Changed {ValueMember.DisplayName}");

            SerializedProperty.boxedValue = newVal;
        }
    }
}