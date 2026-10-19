namespace UnnamedUiToolkit
{
    public interface ISerializeUiToolkitMember : IUiToolkitMember
    {
        public string SerializedPropertyPath { get; }
        public object OnValueLoad(object value);
        public void OnValueChange(object instance, object value);
    }
}