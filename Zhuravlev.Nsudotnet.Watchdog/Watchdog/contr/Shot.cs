using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Drawing;
using System.Web.Routing;

namespace Watchdog.Controllers
{
    class Shot : ApiController
    {
        [HttpGet]
        [Route("Shot")]
        public HttpResponseMessage Get()
        {
            var memoryStream = new MemoryStream();
            var response = new HttpResponseMessage();
            var photo = PhotoCatcher.GetPhotoCatcher().GetPhoto();
            photo.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            response.Content = new StreamContent(memoryStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return response;
        }
    }
}
