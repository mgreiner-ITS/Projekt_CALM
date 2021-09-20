
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace DataAccess
{
    public interface IODB
    {
        bool Connection();
        void InsertData(FileItem newItem);
        List<FileItem> GetFileItemsByText(string searchText);
        void DelItems(FileItem fileItem);
        void UpdateItems(FileItem currentItem, string oldPath);
    }
}
