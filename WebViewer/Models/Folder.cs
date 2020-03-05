using System.Collections.Generic;

namespace WebViewer.Models
{
    public interface IFolder
    {
        string Root { get; set; }
        string Name { get; set; }
        string Path { get; set; }
        List<Folder> Children { get; set; }
    }

    public class Folder : IFolder
    {
        public string Root { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<Folder> Children { get; set; } = new List<Folder>();
    }
}