using System.Threading.Tasks;
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

        public async Task Start()
        {
            await _server.OpenAsync();
        }

        public async Task Stop()
        {
            await _server.CloseAsync();
            _server.Dispose();
            PhotoCatcher.Instance.Dispose();
        }

        private static void Main()
        {
            HostFactory.Run(x =>
            {
                x.Service<MainService>(s =>
                {
                    s.ConstructUsing(name => new MainService());
                    s.WhenStarted(svc => svc.Start().Wait());
                    s.WhenStopped(svc => svc.Stop().Wait());
                });

                x.RunAsLocalSystem();
                x.SetDescription("Watchdog");
                x.SetDisplayName("Watchdog");
                x.SetServiceName("Watchdog");
            });
        }

    }
}
