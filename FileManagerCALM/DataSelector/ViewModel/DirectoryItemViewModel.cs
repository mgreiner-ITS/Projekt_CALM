using BusinessLogic;
using BusinessLogic.Management.FileManagement;
using CommandHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DataSelector.ViewModel
{
    public class DirectoryItemViewModel : ItemViewModel
    {
        private const string FolderIcon = "pack://application:,,,/Resources/folder_blue.png";
        private readonly FileFinder _findFinder;

        public ObservableCollection<ItemViewModel> SubItemViewModels { get; set; } = new ObservableCollection<ItemViewModel>();

        public DirectoryItemViewModel()
        {
            _findFinder = new FileFinder();
            IconPath = FolderIcon;
        }

        public DirectoryItemViewModel(string directoryPath)
        {
            _findFinder = new FileFinder();
            IconPath = FolderIcon;
            Path = directoryPath;
            Name = System.IO.Path.GetFileName(directoryPath);
            
        }

        public DirectoryItemViewModel(DirectoryInfo directoryInfo)
        {
            _findFinder = new FileFinder();
            IconPath = FolderIcon;
            Name = System.IO.Path.GetFileName(directoryInfo.Name);
            Path = directoryInfo.FullName;
        }

        public void InitializeSubItems()
        {
            SubItemViewModels.Clear();
            var items = _findFinder.FindFiles(new DirectoryInfo(Path));

            foreach (var directory in items.directories)
            {
                DirectoryItemViewModel directoryItemViewModel = new DirectoryItemViewModel(directory);
                SubItemViewModels.Add(directoryItemViewModel);
            }

            foreach (var file in items.files)
            {
                FileItemViewModel fileItemViewModel = new FileItemViewModel(new FileInfo(file));
                SubItemViewModels.Add(fileItemViewModel);
            }
        }

        /// <summary>
        ///  To get all files in the directory and in its subfolders
        /// </summary>
        /// <param name="path"></param>
        /// <returns><c>List<FileItemViewModel></c></returns>
        public List<FileItemViewModel> GetAllFiles(string path)
        {
            List<FileItemViewModel> files = new List<FileItemViewModel>();
            try
            {
                foreach (var filePath in Directory.GetFiles(path))
                {
                    InitializeSubItems();
                    var fileToAdd = (FileItemViewModel)SubItemViewModels.Where(item => item.Path.Equals(filePath)).FirstOrDefault();
                    if(fileToAdd != null)
                        files.Add(fileToAdd);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            try
            {
                foreach (var directoryPath in Directory.GetDirectories(path))
                {
                    files.AddRange(GetAllFiles(directoryPath));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return files;
        }

    }
}
