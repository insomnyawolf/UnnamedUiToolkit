using System.Linq.Expressions;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public delegate void OnChange(object instance);

    public class OnChangeAttribute : UnnamedUiToolkitValueAttribute
    {
        public string MethodName { get; }
        public OnChangeAttribute(string MethodName) => this.MethodName = MethodName;
    }

    public static class OnChangeExtensions
    {
        public static Expression<OnChange> ProcessOnChangeAttribute(this OnChangeAttribute OnChange, MemberInfo MemberInfo)
        {
            var targetMethodName = OnChange.MethodName;

            var declaring = MemberInfo.DeclaringType;

            var method = declaring.GetMethod(name: targetMethodName, bindingAttr: GenericReflectionHelpers.ReflectionFlags);

            if (method is null) return null;

            var expr = method.CreateUniversalDelegate<OnChange>(isWeakBind: true);

            return expr;
        }
    }
}