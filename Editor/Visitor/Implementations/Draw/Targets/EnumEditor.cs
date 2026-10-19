using System;

namespace UnnamedUiToolkit
{
    public class EnumEditor : DrawerVisitorTarget
    {
        public static bool IsTarget(Type MemberType)
        {
            if (!MemberType.IsEnum) return false;

            return true;
        }

        protected static InspectorBinding Cache = InspectorBindingCache.EnumDrawer;
        protected static InspectorBindingDelegate Compiled = Cache.Compiled;

        public EnumEditor(IUiToolkitValueMember Member)
            : base(Member) { }

        public override DrawerVisitor RunNode(DrawerVisitor Visitor)
        {
            var values = Visitor.Values;

            var newVal = Compiled(Member.DisplayName, values.Value, EmptyGuiLayoutOptions);

            values.Update(newVal);

            return Visitor;
        }
    }
}