using System;

namespace UnnamedUiToolkit
{
    public class PrimitiveEditor : DrawerVisitorTarget
    {
        // Simple drawer mapping based on type
        protected Func<object, object> CachedDrawer { get; set; }
        protected InspectorBinding InspectorBinding { get; set; }

        public PrimitiveEditor(IUiToolkitValueMember Member, InspectorBinding InspectorBinding)
            : base(Member)
        {
            this.InspectorBinding = InspectorBinding;

            // IdkHow To Avoid The Capturing
            object DrawPrimitive(object value)
            {
                var val = InspectorBinding.Compiled(Member.DisplayName, value, Constants.EmptyGuiLayoutOptions);

                return val;
            }

            CachedDrawer = DrawPrimitive;
        }

        public override NodeVisitor RunNode(NodeVisitor Visitor)
        {
            var values = Visitor.Values;

            var newVal = CachedDrawer(values.Value);

            values.Update(newVal);

            return Visitor;
        }
    }
}