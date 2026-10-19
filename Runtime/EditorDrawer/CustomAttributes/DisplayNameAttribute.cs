using System;

[AttributeUsage(validOn: AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class DisplayNameAttribute : UnnamedUiToolkitAttribute
{
    public string Name { get; }
    public DisplayNameAttribute(string Name = null) => this.Name = Name;
}
