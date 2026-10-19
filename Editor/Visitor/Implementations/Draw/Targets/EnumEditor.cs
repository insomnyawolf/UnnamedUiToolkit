namespace UnnamedUiToolkit
{
    public class EnumEditor : DrawerVisitorTarget
    {
        protected static InspectorBinding Cache = InspectorBindingCache.EnumDrawer;
        protected static InspectorBindingDelegate Compiled = Cache.Compiled;

        public EnumEditor(IUiToolkitValueMember Member)
            : base(Member) { }

        public override NodeVisitor RunNode(NodeVisitor Visitor)
        {
            var values = Visitor.Values;

            var newVal = Compiled(Member.DisplayName, values.Value, Constants.EmptyGuiLayoutOptions);

            values.Update(newVal);

            return Visitor;
        }
    }
}