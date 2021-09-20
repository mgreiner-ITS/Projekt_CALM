using DataAccess;
using Models;

namespace BusinessLogic.Management
{
    public class UploadManagement
    {
        private readonly DB database;

        public UploadManagement()
        {
            database = new DB();
        }

        public void InsertItem(FileItem file)
        {
            database.InsertData(file);
        }
    }
}
