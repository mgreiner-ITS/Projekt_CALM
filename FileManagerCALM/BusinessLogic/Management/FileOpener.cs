﻿using System.Diagnostics;
using Models;
using System;
namespace BusinessLogic.Management
{
    public class FileOpener
    {

        public FileOpener( FileItem fileItem)
        {
            ProcessStartInfo StartInformation = new ProcessStartInfo();

            StartInformation.FileName = fileItem.Path;
            try
            {
                Process process = Process.Start(StartInformation);
                process.EnableRaisingEvents = true;
            }
            catch (Exception)
            {

                throw;
            }
            

           
        }

    }
}