using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebViewer.Services;

namespace WebViewer.Controllers {

    [Produces (MediaTypeNames.Application.Json)]
    [ApiController]
    [Route ("api/[controller]")]
    public class ImagesController : ControllerBase {
        IImageService _imageservice;
        ILogger<ImagesController> _logger;

        public ImagesController (
            IImageService imageService,
            ILogger<ImagesController> logger) {
            _imageservice = imageService;
            _logger = logger;
        }

        [Route ("ping")]
        public IActionResult Ping () {
            return Ok (_imageservice.Ping ());
        }

        [Route ("folders")]
        public IActionResult GetFolders (string folder = null) {
            var folders = _imageservice.GetFolders (folder);
            return Ok (folders);
        }

        [Route ("files")]
        public IActionResult GetFiles (string folder = null) {
            var files = _imageservice.GetImages (folder);
            return Ok (files);
        }

        [Route ("thumbnail")]
        public FileResult GetThumbnail (string file = null, int quality = 50, int size = 50) {
            var bytes = _imageservice.GetThumbnail (file, quality, size);
            return File (bytes, "image/jpg");
        }
    }
}