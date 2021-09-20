using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Events
{
    public delegate void InfosMessageEventHandler(string msg);
    public delegate void ErrorMessageEH(string msg);
}
