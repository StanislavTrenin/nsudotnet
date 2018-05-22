using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Watchdog.Controllers
{
    public class IndexController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage
            {
                Content = new StringContent(
                    "<html><head><title>Watchdog</title></head><body><a href = \"/Shot\">GetPhoto</a></body>")
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
