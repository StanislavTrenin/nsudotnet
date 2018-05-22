using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Routing;

namespace Watchdog.Controllers
{
    class Index : ApiController
    {
        [HttpGet]
        [Route("Index")]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("<html><head><title>Watchdog</title></head><body><a href = \"/Shot\">GetPhoto</a></body>");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
