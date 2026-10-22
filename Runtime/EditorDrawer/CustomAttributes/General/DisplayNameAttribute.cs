namespace UnnamedUiToolkit
{
    public class DisplayNameAttribute : UnnamedUiToolkitAttribute
    {
        public string Name { get; }
        public DisplayNameAttribute(string Name = null) => this.Name = Name;
    }
}