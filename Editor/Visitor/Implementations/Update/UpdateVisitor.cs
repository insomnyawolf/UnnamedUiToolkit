namespace UnnamedUiToolkit
{
    public class UpdateVisitor : NodeVisitor<UpdateVisitor>
    {
        public UpdateVisitor() {}

        public override void Visit(IUiToolkitBase Target)
        {
            if (Target is IUiToolkitUpdate updatable)
            {
                updatable.Update(this);
            }
        }
    }
}