using DataAccess;


namespace BusinessLogic.Management
{
    public class SearchManagement
    {
        DB dbAcessSql;

        public SearchManagement()
        {
            dbAcessSql = new DB();

        }

        public void SearchText(string sucheText)
        {

            dbAcessSql.GetFileItems(sucheText);

        }
    }
}
