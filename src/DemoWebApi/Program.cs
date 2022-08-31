using DemoWebApi;

namespace DemoWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Bootstrap.Run(args);
        }
    }
}
