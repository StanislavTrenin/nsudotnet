using System.Web.Http;
using System.Web.Http.SelfHost;
using Topshelf;

namespace Watchdog
{
    public class MainService
    {

        private readonly HttpSelfHostServer _server;

        public MainService()
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080");
            config.Routes.MapHttpRoute("API Default", "{controller}", new { controller = "Index" });
            _server = new HttpSelfHostServer(config);
        }

        public void Start()
        {
            _server.OpenAsync();
        }

        public void Stop()
        {
            _server.CloseAsync();
            _server.Dispose();
        }

        private static void Main()
        {
            HostFactory.Run(x =>
            {
                x.Service<MainService>(s =>
                {
                    s.ConstructUsing(name => new MainService());
                    s.WhenStarted(svc => svc.Start());
                    s.WhenStopped(svc => svc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("Watchdog");
                x.SetDisplayName("Watchdog");
                x.SetServiceName("Watchdog");
            });
        }

    }
}
