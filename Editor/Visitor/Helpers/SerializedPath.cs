namespace UnnamedUiToolkit
{
    public class SerializedPath
    {
        public string Path { get; set; } = "";
        public string SubPath { get; set; } = "";
        public string FullPath => GetFullPath();

        protected string GetFullPath()
        {
            var separator = string.IsNullOrEmpty(Path) ? "" : ".";

            var value = $"{Path}{separator}{SubPath}";

            return value;
        }

        public SerializedPath GetRelativeChildren()
        {
            var path = FullPath;

            var value = new SerializedPath()
            {
                Path = path,
            };

            return value;
        }
    }
}