using DataAccess;
using Models;
using System;
using System.IO;

namespace BusinessLogic.Management
{
    public class UploadManagement
    {
        private readonly DatabaseConnection database;

        public UploadManagement()
        {
            database = new DatabaseConnection();
        }

        public void UploadFile( FileItem fileItemProxy )
        {
            string currentFullPath = fileItemProxy.Path;
            DateTime currentLastModified = fileItemProxy.LastModified;

            FileReader fileReader = new FileReader();
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
            catch (FileNotFoundException)
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
