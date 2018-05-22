using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Watchdog
{
    internal class Program
    {
        private static void Main()
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080");
            config.Routes.MapHttpRoute("API Default", "{controller}", new { controller = "Index" });
            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
