using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class DirectoryItem
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public List<DirectoryItem> SubDirectories { get; set; } = new List<DirectoryItem>();
        public List<FileItem> Files { get; set; } = new List<FileItem>();

    }
}
