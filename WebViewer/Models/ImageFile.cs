namespace WebViewer.Models
{
    public interface IImageFile
    {
        Folder Folder { get; set; }
        string Name { get; set; }
        string RelativeUrl { get; set; }
        string Path { get; }
    }

    public class ImageFile : IImageFile
    {
        public Folder Folder { get; set; }
        public string Name { get; set; }
        public string RelativeUrl { get; set; }

        public string Path { get { return System.IO.Path.Combine(Folder?.Path, Name); } }
    }
}