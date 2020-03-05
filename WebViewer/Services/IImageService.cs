using System.Collections.Generic;
using WebViewer.Models;

namespace WebViewer.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IImageService {
        object Ping ();
        List<Folder> GetFolders (string folder = null);
        List<ImageFile> GetImages (string folder);
        public byte[] GetThumbnail  (string inputPath, int quality = 50, int size = 50);
    }
}