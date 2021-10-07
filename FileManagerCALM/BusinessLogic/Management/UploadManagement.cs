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

        public void UploadFile( FileItem fileItemProxy )
        {
            string currentFullPath = fileItemProxy.Path;
            DateTime currentLastModified = fileItemProxy.LastModified;

            FileItemReader fileReader = new FileItemReader();
            var fileItem = fileReader.ReadFile(currentFullPath, fileItemProxy.LastModified);

            try
            {
                DateTime dbLastModified = database.GetFileLastModified(currentFullPath);
                if ( !AreDatesEqual( currentLastModified, dbLastModified ) )
                {
                    database.UpdateItems(fileItem, fileItem.Path);
                }
                else
                {
                    //Do nothing, identical file already exists in the database
                }
            }
            catch (System.Exception)
            {
                database.InsertData(fileItem);
            }
        }

        private bool AreDatesEqual( DateTime dateTime, DateTime otherDateTime)
        {
            if (dateTime.Year == otherDateTime.Year && dateTime.Month == otherDateTime.Month 
                && dateTime.Day == otherDateTime.Day && dateTime.Hour == otherDateTime.Hour 
                && dateTime.Minute == otherDateTime.Minute && dateTime.Second == otherDateTime.Second)
            {
                return true;
            }
            return false;
        }
    }
}
