using System;

namespace Models
{
    public class FileItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public FileType Type { get; set; }
        public string Content { get; set; }
        public string Path { get; set; }
        public string Partition { get; set; }
        public DateTime LastModified { get; set; }
    }



}
