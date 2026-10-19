namespace UnnamedUiToolkit
{
    public interface IUiToolkitDraw : IUiToolkitElement
    {
        NodeVisitor Draw(NodeVisitor Visitor);
    }
}