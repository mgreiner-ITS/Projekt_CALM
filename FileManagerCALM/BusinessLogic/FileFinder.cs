using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BusinessLogic
{
    public class FileFinder
    {
        public List<DriveInfo> FindPartitions()
        {
            return DriveInfo.GetDrives().ToList();
        }

        public (List<string> directories, List<string> files) FindFiles(DriveInfo driveInfo)
        {
            List<string> allDirectories = new List<string>();
            List<string> allFiles = new List<string>();
            try
            {
                allDirectories = Directory.GetDirectories(driveInfo.Name).ToList(); // Unter Ordner
                allFiles = Directory.GetFiles(driveInfo.Name).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return (allDirectories, allFiles);
        }

        public (List<string> directories, List<string> files) FindFiles(DirectoryInfo directoryInfo)
        {
            List<string> allDirectories = new List<string>();
            List<string> allFiles = new List<string>();
            try
            {
                allDirectories = Directory.GetDirectories(directoryInfo.FullName).ToList();
                allFiles = Directory.GetFiles(directoryInfo.FullName).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return (allDirectories, allFiles);
        }

        //TODO: Will be needed later for the service

        //public void MonitorFileSystem(string driveLetter)
        //{
        //    FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(driveLetter);
        //    fileSystemWatcher.Changed += FileChanged;
        //    fileSystemWatcher.Deleted += FileDeleted;
        //    fileSystemWatcher.Renamed += FileRenamed;
        //    fileSystemWatcher.Created += FileCreated;

        //    fileSystemWatcher.IncludeSubdirectories = true;
        //    fileSystemWatcher.EnableRaisingEvents = true;
        //}

        //private void FileCreated(object sender, FileSystemEventArgs e)
        //{
        //}

        //private void FileRenamed(object sender, RenamedEventArgs e)
        //{
        //}

        //private void FileDeleted(object sender, FileSystemEventArgs e)
        //{
        //}

        //private void FileChanged(object sender, FileSystemEventArgs e)
        //{
        //}
    }
}
