using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ProductCatalogApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Pic")]
    public class PicController : Controller
    {
        private readonly IHostingEnvironment _env;
        public PicController(IHostingEnvironment env)
        {
            _env = env;
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetImage(int id)
        {
            // IHostEnvironment Object is available in our project
            // When an instance is injected into our class
            // Provides us: command line, json, docker file, 
            // path to the root folder from anywhere
            var webRoot = _env.WebRootPath;
            // Find the full path of the image
            var path = Path.Combine(webRoot + "/Pics/","shoes-" + id + ".png");
            // Convert to an image and return it
            // Create an image/file buffer
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer,"image/png");
        }
    }
}
