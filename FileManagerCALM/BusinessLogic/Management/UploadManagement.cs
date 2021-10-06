using DataAccess;
using Models;
using System;

namespace BusinessLogic.Management
{
    public class UploadManagement
    {
        private readonly DB database;

        public UploadManagement()
        {
            database = new DB();
        }

        public void InsertFile(FileItem file)
        {
            database.InsertData(file);
        }

        /// <summary>
        /// Throws an Exception when no file was found
        /// </summary>
        public DateTime SearchLastModified(string fullFilePath)
        {
            return database.GetFileLastModified(fullFilePath);
        }

        public void UpdateFile(FileItem file)
        {
            database.UpdateItems(file, file.Path);
        }
    }
}
