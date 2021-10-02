using DataAccess;
using Models;
using System.Collections.Generic;

namespace BusinessLogic.Management
{
    public class SearchManagement
    {
        DB dbAcessSql;
        List<FileItem> listItem;
        public SearchManagement()
        {
            dbAcessSql = new DB();

        }

        public List<FileItem> SearchText(string searchText)
        {
            listItem = dbAcessSql.GetFileItemsByText(searchText);
            return listItem;

        }
    }
}
