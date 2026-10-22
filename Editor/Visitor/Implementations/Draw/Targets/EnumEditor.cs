using System;

namespace UnnamedUiToolkit
{
    public class EnumEditor : DrawerVisitorTarget
    {
        protected static InspectorBinding BaseCache = InspectorBindingCache.EnumDrawer;
        public EnumEditor(IUiToolkitValueMember Member)
            : base(Member) { }

        public override NodeVisitor RunNode(NodeVisitor Visitor)
        {
            var values = Visitor.Values;

            var preValue = Enum.ToObject(Member.MemberType, values.Value);
            
            var newVal = BaseCache.Compiled(Member.DisplayName, preValue, Constants.EmptyGuiLayoutOptions);

            values.Update(newVal);

            return Visitor;
        }
    }
}