namespace UnnamedUiToolkit
{
    public abstract class BaseVisitorTarget
    {
        public IUiToolkitValueMember Member { get; }

        public BaseVisitorTarget(IUiToolkitValueMember Member)
        {
            this.Member = Member;
        }

        public abstract NodeVisitor RunNode(NodeVisitor value);
    }
}
