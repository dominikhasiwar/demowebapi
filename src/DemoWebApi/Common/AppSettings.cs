using System.Reflection;

namespace DemoWebApi.Common
{
    public class AppSettings
    {
        static AppSettings()
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Current = Config.Get<AppSettings>();
        }

        public static AppSettings Current { get; }

        public static IConfigurationRoot Config { get; }

        public string BaseAddress { get; set; }
    }
}
