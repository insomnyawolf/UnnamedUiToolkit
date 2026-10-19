using System;

namespace UnnamedUiToolkit
{
    public class ChildVisitorTarget<TVisitor> : BaseVisitorTarget
        where TVisitor : NodeVisitor, new()
    {
        protected TreeHandler Cached { get; set; }
        protected Type MemberType => Member.MemberType;
        public ChildVisitorTarget(IUiToolkitValueMember Member)
             : base(Member) { Cached = CacheChildInspector(); }

        public override NodeVisitor RunNode(NodeVisitor Visitor)
        {
            var values = Visitor.Values;

            var val = values.Value;

            val ??= Activator.CreateInstance(MemberType);

            values.Update(val);

            var childVisitor = Visitor.GetChildVisitor<TVisitor>();

            Cached.VisitNodes(childVisitor);

            return Visitor;
        }

        protected TreeHandler CacheChildInspector()
        {
            var childEditor = new TreeHandler(MemberType);

            // Maybe cache yet?
            childEditor.Initialize();

            return childEditor;
        }
    }
}