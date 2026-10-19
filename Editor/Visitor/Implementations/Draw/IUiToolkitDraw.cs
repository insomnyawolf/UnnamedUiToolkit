namespace UnnamedUiToolkit
{
    public interface IUiToolkitDraw : IUiToolkitElement
    {
        DrawerVisitor Draw(DrawerVisitor Visitor);
    }
}