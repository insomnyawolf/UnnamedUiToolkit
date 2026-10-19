using UnityEditor;

namespace UnnamedUiToolkit
{
    public class UpdateVisitor : NodeVisitor
    {
        public UpdateVisitor() { }

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

            // No value, nothing to update
        }

        public void ProcessValueMember(IUiToolkitValueMember ValueMember)
        {
            if (ValueMember.MemberType.IsArray())
            {
                return;
            }

            if (GetPropertyIfSerializable() is not SerializedProperty SerializedProperty)
            {
                // Not serialized fields doesn't need this fix
                // All members updated instantly in the code and interface
                // But those updates doesn't automatically carry to serialized properties
                // That is why this exist
                return;
            }

            if (ValueMember.MemberType.IsChild())
            {
                Values = new Values();

                var V0 = ValueMember.GetExpression.Compiled(Instance);

                Values.Update(V0);

                var cu = new ChildUpdater(ValueMember);

                cu.RunNode(this);
                
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