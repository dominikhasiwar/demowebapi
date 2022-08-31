namespace DemoWebApi.Common
{
    public class AppInfo
    {
        public string AppName { get; } = "DemoWebApi";

        public string AppVersion { get; } = "1.0.0";

        public static AppInfo Current { get; } = new AppInfo();

        private AppInfo()
        {
        }
    }
}
