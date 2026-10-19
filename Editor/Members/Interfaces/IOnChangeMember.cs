namespace UnnamedUiToolkit
{
    public interface IOnChangeMember : IUiToolkitValueMember
    {
        public DynamicCall<OnChange> OnChangeExpr { get; }
    }
}