using DataAccess;
using Models;
using System.Collections.Generic;

namespace BusinessLogic.Management
{
    public class SearchManagement
    {
        DB dbAcessSql;

        public SearchManagement()
        {
            dbAcessSql = new DB();

        }

        public List<FileItem> SearchText(string searchText)
        {

            return dbAcessSql.GetFileItemsByText(searchText);

        }
    }
}
