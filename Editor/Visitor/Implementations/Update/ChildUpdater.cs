namespace UnnamedUiToolkit
{
    public class ChildUpdater : ChildVisitorTarget<UpdateVisitor>
    {
        public ChildUpdater(IUiToolkitValueMember Member)
             : base(Member) { }
    }
}