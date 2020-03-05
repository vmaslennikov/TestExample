using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using WebViewer.Models;

namespace WebViewer.Services
{
    /// <summary>
    /// Сервис работы с картинками
    /// </summary>
    public class ImageService : IImageService {
        const string cacheImageFolder = "wwwroot\\cache";
        const string rootImageFolder = "wwwroot\\images";
        const string rootImageFolderRelative = "/wwwroot/images";
        private readonly ILogger<ImageService> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImageService (
            IWebHostEnvironment hostingEnvironment,
            ILogger<ImageService> logger) {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        /// <summary>
        /// Ping
        /// </summary>
        /// <returns></returns>
        public object Ping () {
            _logger.LogInformation ($"{nameof(Ping)} call");
            return new {
                UtcTime = DateTime.UtcNow,
                    Path = _hostingEnvironment?.WebRootPath,
                    Value = "pong"
            };
        }

        public List<Folder> GetFolders (string folder = null) {
            _logger.LogInformation ($"{nameof(GetFolders)} call");
            var folders = new List<Folder> ();
            string filepath = GetTargetPath (folder);
            folders.AddRange (GetDirectoryTree (filepath));
            return folders;
        }

        public List<ImageFile> GetImages (string folder = null) {
            _logger.LogInformation ($"{nameof(GetImages)} call");
            var files = new List<ImageFile> ();
            string filepath = string.IsNullOrEmpty (folder) ? GetTargetPath (folder) : folder;
            if (Directory.Exists (filepath)) {
                var children = Directory.GetFiles (filepath);
                var f = new Folder {
                    Name = Path.GetFileName (filepath),
                    Path = filepath
                };
                foreach (var item in children) {
                    files.Add (new ImageFile {
                        Folder = f,
                            Name = Path.GetFileName (item),
                    });
                }
            }
            return files;
        }

        private static List<Folder> GetDirectoryTree (string f, string parent = null) {
            var folders = new List<Folder> ();
            if (Directory.Exists (f)) {
                var children = Directory.GetDirectories (f);
                foreach (var item in children) {
                    var temp = new Folder {
                        Name = Path.GetFileName (item),
                        Path = item,
                        Root = string.IsNullOrEmpty (parent) ?
                        $"{rootImageFolderRelative}/{Path.GetFileName (item)}" :
                        $"{parent}/{Path.GetFileName (item)}"
                    };
                    temp.Children.AddRange (GetDirectoryTree (Path.Combine (f, temp.Name), temp.Root));
                    folders.Add (temp);
                }
            }
            return folders;
        }
        
        private static string GetTargetPath (string folder) {
            var root = Directory.GetCurrentDirectory ();
            var filepath = Path.Combine (root, rootImageFolder);
            if (!string.IsNullOrEmpty (folder)) {
                filepath = Path.Combine (filepath, folder);
            }

            return filepath;
        }

        public byte[] GetThumbnail (string inputPath, int quality = 50, int size = 50) {
            byte[] result = null;
            var root = Directory.GetCurrentDirectory ();
            var filepath = Path.Combine (root, cacheImageFolder);
            if(!Directory.Exists(filepath)){
                Directory.CreateDirectory(filepath);
            }
            var dir = Path.GetDirectoryName(inputPath);
            var filename = Path.GetFileNameWithoutExtension(inputPath);
            var extension = Path.GetExtension(inputPath);
            var output = $"{filepath}\\{filename}_{quality}_{size}{extension}";
            if(File.Exists(output)){
                return File.ReadAllBytes(output);
            }
            using (var image = new MagickImage (inputPath)) {
                image.Resize (size, size);
                image.Strip ();
                image.Quality = quality;
                using (var memoryStream = new MemoryStream ()) {
                    image.Write (memoryStream);
                    image.Write(output);
                    result = memoryStream.ToArray ();
                }
            }
            return result;
        }
    }
}