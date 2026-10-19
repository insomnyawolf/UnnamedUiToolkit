using UnityEditor;

namespace UnnamedUiToolkit
{
    public abstract class DrawerVisitorTarget : BaseVisitorTarget<DrawerVisitor>
        //where TIUiToolkit : IUiToolkitBase
    {
        protected DrawerVisitorTarget(IUiToolkitValueMember Member) : base(Member) { }
    }
}