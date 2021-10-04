using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;

namespace BusinessLogic.Management
{
    public class MonitoringManagement
    {
        private readonly DB _db;

        private List<FileItem> monitoredFiles = new List<FileItem>();
        private List<string> monitoredDirectoryPaths = new List<string>();

        public MonitoringManagement()
        {
            _db = new DB();
        }

        //TODO: Currently a moved file is deleted from the database and thus no longer tracked
        public void InitialzeWatcher()
        {
            try
            {
                foreach (var fileItem in _db.GetAllFileItems())
                {
                    FileInfo fileInfo = new FileInfo(fileItem.Path);

                    if (!monitoredDirectoryPaths.Contains(fileInfo.Directory.FullName))
                    {
                        FileSystemWatcher watcher = new FileSystemWatcher(fileInfo.Directory.FullName);

                        watcher.Created += Watcher_Created;
                        watcher.Deleted += Watcher_Deleted;
                        watcher.Renamed += Watcher_Renamed;
                        watcher.EnableRaisingEvents = true;

                        monitoredDirectoryPaths.Add(fileInfo.Directory.FullName);
                    }

                    monitoredFiles.Add(fileItem);
                }
            }
            catch (SecurityException)
            {
                Debug.Write("The caller does not have the required permission.");
            }
            catch (NotSupportedException)
            {
                Debug.Write("FileName contains a colon (:) in the middle of the string.");
            }
            catch (ArgumentException)
            {
                Debug.Write(".NET Framework and .NET Core versions older than 2.1: The file name is empty, contains only white spaces, or contains invalid characters.");
            }
            catch (UnauthorizedAccessException)
            {
                Debug.Write("Access to fileName is denied.");
            }
            catch (PathTooLongException)
            {
                Debug.Write("The specified path, file name, or both exceed the system-defined maximum length.");
            }
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            var fileItem = FindFileItem(e.OldFullPath);

            if (fileItem != null)
                _db.UpdateItems(fileItem, e.OldFullPath);
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            var fileItem = FindFileItem(e.FullPath);

            if (fileItem != null)
                _db.DelItems(fileItem);
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            var fileItem = FindFileItem(e.FullPath);

            if (fileItem != null)
                _db.InsertData(fileItem);
        }

        private FileItem FindFileItem(string path)
        {
            return monitoredFiles.FirstOrDefault(monitoredFile => monitoredFile.Path == path);
        }
    }
}
