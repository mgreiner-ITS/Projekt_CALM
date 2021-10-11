using System.Diagnostics;
using Models;
using System;
namespace BusinessLogic.Management.FileManagement
{
    public class FileOpener
    {

        public FileOpener( FileItem fileItem)
        {
            ProcessStartInfo StartInformation = new ProcessStartInfo();
            if(fileItem != null)
            {
                StartInformation.FileName = fileItem.Path;
                try
                {
                    Process process = Process.Start(StartInformation);
                    process.EnableRaisingEvents = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
           
        }

    }
}
