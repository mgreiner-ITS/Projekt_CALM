using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class File
    {
        public string Name { get; set; }
        public string Typ { get; set; }
        public string Text { get; set; }
        public string Pfad { get; set; }

        Status _status;

        public FileType getTyp()
        {

            switch (this.Typ)
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
