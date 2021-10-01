using BusinessLogic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace DataSelector.ViewModel
{
    public class DirectoryItemViewModel : ItemViewModel
    {
        private readonly FileFinder _findFinder;
        public ObservableCollection<ItemViewModel> SubItemViewModels { get; set; } = new ObservableCollection<ItemViewModel>();

        public DirectoryItemViewModel()
        {
            _findFinder = new FileFinder();
        }

        public DirectoryItemViewModel(DirectoryInfo directoryInfo)
        {
            _findFinder = new FileFinder();
            Name = directoryInfo.Name;
            Path = directoryInfo.FullName;
            InitializeSubItems();
        }

        private void InitializeSubItems()
        {
            SubItemViewModels.Clear();
            var items = _findFinder.FindFiles(new DirectoryInfo(Path));

            foreach (var directory in items.directories)
            {
                DirectoryItemViewModel directoryItemViewModel = new DirectoryItemViewModel(new DirectoryInfo(directory));
                SubItemViewModels.Add(directoryItemViewModel);
            }

            foreach (var file in items.files)
            {
                FileItemViewModel fileItemViewModel = new FileItemViewModel(new FileInfo(file));
                SubItemViewModels.Add(fileItemViewModel);
            }
        }

        public List<FileItemViewModel> GetFiles()
        {
            List<FileItemViewModel> files = new List<FileItemViewModel>();
            foreach (var filePath in Directory.GetFiles(Path))
            {
                files.Add(new FileItemViewModel(new FileInfo(filePath)));
            }
            return files;
        }
    }
}
