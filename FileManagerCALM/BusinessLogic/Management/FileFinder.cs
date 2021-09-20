using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BusinessLogic.Management
{
    public class FileFinder
    {
        public List<DriveInfo> FindPartitions()
        {
            return DriveInfo.GetDrives().ToList();
        }

        public (List<DirectoryInfo> directories, List<FileInfo> files) FindFiles(DriveInfo driveInfo)
        {
            var allDirectoryPaths = Directory.GetDirectories(driveInfo.Name, "*", SearchOption.TopDirectoryOnly).ToList(); // Unter Ordner
            var directoryInfos = new List<DirectoryInfo>();
            foreach (var directoryPath in allDirectoryPaths)
            {
                DirectoryInfo directory = new DirectoryInfo(directoryPath);
                //if(!directory.Attributes.HasFlag(FileAttributes.Hidden) && directory.Attributes.HasFlag(FileAttributes.System))
                directoryInfos.Add(directory);
            }
            var allFilePaths = Directory.GetFiles(driveInfo.Name).ToList();
            var fileInfos = new List<FileInfo>();
            foreach (var filePath in allFilePaths)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                fileInfos.Add(fileInfo);
            }
            return (directoryInfos, fileInfos);
        }

        public (List<DirectoryInfo> directories, List<FileInfo> files) FindFiles(DirectoryInfo directoryInfo)
        {
            var allDirectoryPaths = Directory.GetDirectories(directoryInfo.FullName).ToList();

            var directoryInfos = new List<DirectoryInfo>();
            foreach (var directoryPath in allDirectoryPaths)
            {
                DirectoryInfo directory = new DirectoryInfo(directoryPath);
                //if (!directory.Attributes.HasFlag(FileAttributes.Hidden) && directory.Attributes.HasFlag(FileAttributes.System))
                directoryInfos.Add(directory);
            }

            var allFilePaths = Directory.GetFiles(directoryInfo.FullName).ToList();
            var fileInfos = new List<FileInfo>();
            foreach (var filePath in allFilePaths)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                fileInfos.Add(fileInfo);
            }
            return (directoryInfos, fileInfos);
        }
    }
}
