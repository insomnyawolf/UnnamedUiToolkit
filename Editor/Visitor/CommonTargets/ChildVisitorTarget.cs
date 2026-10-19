using System;

namespace UnnamedUiToolkit
{
    public class ChildVisitorTarget<TVisitor> : BaseVisitorTarget<TVisitor>
        where TVisitor : NodeVisitor<TVisitor>, new()
    {
        public static bool IsTarget(Type MemberType)
        {
#warning This check is not good enough

            // Structs?
            // This is pain
            if (!MemberType.IsClass)
            {
                return false;
            }

            if (MemberType.IsArray)
            {
                return false;
            }

            if (MemberType.IsEnum)
            {
                return false;
            }

            if (MemberType.IsUnityNativeType())
            {
                return false;
            }

            return true;
        }

        protected TreeHandler Cached { get; set; }
        protected Type MemberType => Member.MemberType;
        public ChildVisitorTarget(IUiToolkitValueMember Member)
             : base(Member) { Cached = CacheChildInspector(); }

        public override TVisitor RunNode(TVisitor Visitor)
        {
            var values = Visitor.Values;

            var val = values.Value;

            val ??= Activator.CreateInstance(MemberType);

            values.Update(val);

            var childVisitor = Visitor.GetChildVisitor();

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