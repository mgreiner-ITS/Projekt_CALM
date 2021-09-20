using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Models
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
    }
}
