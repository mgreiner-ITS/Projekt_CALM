using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class FileItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string Path { get; set; }
        public string Partition { get; set; }
        public DateTime LastModified { get; set; }

        Status _status;

        public FileType getTyp()
        {

            switch (this.Type)
            {
                case "pdf": return FileType.pdf;
                case "txt": return FileType.txt;

                default:
                    return FileType.other;
                    break;
            }


        }

    }
   


}
