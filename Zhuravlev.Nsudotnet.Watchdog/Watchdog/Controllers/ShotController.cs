using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Watchdog.Controllers
{
    public class ShotController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var memoryStream = new MemoryStream();
            var response = new HttpResponseMessage();
            var photo = PhotoCatcher.GetPhotoCatcher().GetPhoto();
            photo.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            memoryStream.Seek(0, SeekOrigin.Begin);
            response.Content = new StreamContent(memoryStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return response;
        }
    }
}
