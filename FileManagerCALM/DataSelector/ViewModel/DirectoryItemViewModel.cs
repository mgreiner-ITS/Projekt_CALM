using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataSelector.ViewModel
{
    class DirectoryItemViewModel : ViewModelBase
    {

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }
        private bool _status;

        public bool Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DirectoryItemViewModel> SubDirectoryViewModels { get; set; } = new ObservableCollection<DirectoryItemViewModel>();
        public ObservableCollection<FileItemViewModel> FileViewModels { get; set; } = new ObservableCollection<FileItemViewModel>();

    }
}
