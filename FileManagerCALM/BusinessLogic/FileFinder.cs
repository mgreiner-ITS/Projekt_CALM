using System.Collections.Generic;
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
            var allDirectories = Directory.GetDirectories(driveInfo.Name).ToList(); // Unter Ordner
            var allFiles = Directory.GetFiles(driveInfo.Name).ToList();
            return (allDirectories, allFiles);
        }

        public (List<string> directories, List<string> files) FindFiles(string drivePath)
        {
            var allDirectories = Directory.GetDirectories(drivePath).ToList();
            var allFiles = Directory.GetFiles(drivePath).ToList();
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
