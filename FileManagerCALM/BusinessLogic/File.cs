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
       public FileTypeEnum Typ { get; set; }
       public string Text { get; set; }
       public string Pfad { get; set; }
        StatusEnum Status;

    }

   
}
