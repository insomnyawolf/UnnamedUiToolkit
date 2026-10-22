using System;
namespace UnnamedUiToolkit
{
    [AttributeUsage(validOn: AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UnnamedUiToolkitValueAttribute : UnnamedUiToolkitAttribute { }
}