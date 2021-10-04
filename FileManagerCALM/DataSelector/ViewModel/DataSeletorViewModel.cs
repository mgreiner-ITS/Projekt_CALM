using BusinessLogic;
using BusinessLogic.Management;
using CommandHelper;
using DataSelector.View;
using Models;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataSelector.ViewModel
{
    public class DataSeletorViewModel : ViewModelBase
    {
        private readonly FileFinder _fileFinder;
        private readonly UploadManagement _uploadManagement;

        private PartitionViewModel _selectedPartition;
        private DirectoryItemViewModel _selectedDirectory;
        public ListBox selectionList;

        private bool cancelledSync;

        public ObservableCollection<PartitionViewModel> Partitions { get; set; } = new ObservableCollection<PartitionViewModel>();

        public ObservableCollection<TreeViewItem> SelectionOverview { get; set; } = new ObservableCollection<TreeViewItem>();

        public ObservableCollection<ItemViewModel> SelectedItemViewModels { get; set; } = new ObservableCollection<ItemViewModel>();

        public List<FileItemViewModel> SelectedFiles { get; set; } = new List<FileItemViewModel>();


        public ICommand WindowLoadedCommand { get; set; }
        public ICommand ShowSearchWindowCmd { get; private set; }
        public RelayCommand SyncCommand { get; set; }
        public RelayCommand CancelSyncCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand SelectAllCommand { get; set; }
        public RelayCommand UnselectAllCommand { get; set; }

        public DataSeletorViewModel()
        {
            _uploadManagement = new UploadManagement();
            ShowSearchWindowCmd = new DelegateCommand(ShowMethod);
            _fileFinder = new FileFinder();

            SyncCommand = new RelayCommand(unused => Sync());
            CancelSyncCommand = new RelayCommand(unused => CancelSync());
            GoBackCommand = new RelayCommand(unused => GoBack());
            SelectAllCommand = new RelayCommand(unused => SelectAll());
            UnselectAllCommand = new RelayCommand(unused => UnselectAll());

            MonitorUsbInputs();


            GetAllPartions();

            SelectionOverview.Add(new TreeViewItem() { Header = "Test" }
                                                       );

            //michael test :)
            //FileItemViewModel fivm = new FileItemViewModel
            //{
            //    Name = "asd",
            //    Path = "c:/asd",
            //    LastModified = new DateTime(),
            //    Partition = "C",
            //    Type = FileType.txt
            //};

            //DirectoryItemViewModel divm = new DirectoryItemViewModel
            //{
            //    Name = "folder",
            //    Path = "c:/folder"
            //};


            //SelectedItemViewModels.Add(fivm);
            //SelectedItemViewModels.Add(divm);
            //SelectedItemViewModels.Add(fivm);
            ////michael test ende :)

            //// Test
            //ProgressBarProgress = 75;

        }



        private void Sync()
        {
            List<ItemViewModel> itemViewModels = SelectedItemViewModels.ToList();
            FileItemReader fileReader = new FileItemReader();
            cancelledSync = false;

            double numberOfItems = itemViewModels.Count();
            double numberOfProcessedItems = 0;
            ProgressBarProgress = 0;
            Task.Run(() =>
            {
                foreach (ItemViewModel currentItemViewModel in itemViewModels)
                {
                    if (cancelledSync)
                        break;

                    if (currentItemViewModel.GetType() == typeof(FileItemViewModel))
                    {
                        ItemViewModelConverter converter = new ItemViewModelConverter();
                        _uploadManagement.InsertItem(fileReader.ReadFile(currentItemViewModel.Name));
                        //TODO die nächste zeile (& somit den converter) braucht man nicht mehr? weil der "convert" automatisch beim read passiert?
                        //_uploadManagement.InsertItem(converter.convert((FileItemViewModel)currentItemViewModel));
                    }

                    numberOfProcessedItems++;
                    ProgressBarProgress = (int)((numberOfProcessedItems / numberOfItems) * 100);
                }
            }
            );
        }

        private void CancelSync()
        {
            cancelledSync = true;
        }
        SearchView objSearchView = null;
        private void ShowMethod()
        {
            if (objSearchView == null )
            {
                objSearchView = new SearchView();
                objSearchView.Closed += (sender, args) => objSearchView = null;
                objSearchView.Show();
            }
        }

        public PartitionViewModel SelectedPartition
        {
            get { return _selectedPartition; }
            set
            {
                _selectedPartition = value;
                if(value != null)
                {
                    SelectedDirectory = null;
                    value.InitializeItems();
                    DetailView = new DirectoryView();
                }
                OnPropertyChanged();
            }
        }

        public DirectoryItemViewModel SelectedDirectory
        {
            get { return _selectedDirectory; }
            set
            {
                _selectedDirectory = value;
                if(value != null)
                {
                    _selectedDirectory.InitializeSubItems();
                }
                OnPropertyChanged();
            }
        }

        //public DirectoryItemViewModel ParentDirectory { get; set; }

        private int _progressBarProgress;
        public int ProgressBarProgress
        {
            get { return _progressBarProgress; }
            set
            {
                _progressBarProgress = value;
                OnPropertyChanged();
            }
        }

        private object _detailView;

        public object DetailView
        {
            get { return _detailView; }
            set
            {
                _detailView = value;
                OnPropertyChanged();
            }
        }

        private void GetAllPartions()
        {
            Partitions.Clear();
            foreach (var item in _fileFinder.FindPartitions())
            {
                PartitionViewModel partitionViewModel = new PartitionViewModel(item);

                Partitions.Add(partitionViewModel);

            }
        }


        public void MonitorUsbInputs()
        {
            ManagementEventWatcher insertWatcher = new ManagementEventWatcher();
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher();
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2");
            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 3");

            insertWatcher.EventArrived += new EventArrivedEventHandler(Watcher_UsbInserted);
            insertWatcher.Query = insertQuery;
            insertWatcher.Start();

            removeWatcher.EventArrived += new EventArrivedEventHandler(Watcher_UsbRemoved);
            removeWatcher.Query = removeQuery;
            removeWatcher.Start();
        }

        private void Watcher_UsbInserted(object sender, EventArrivedEventArgs e) => WatcherLogic();
        private void Watcher_UsbRemoved(object sender, EventArrivedEventArgs e) => WatcherLogic();
        private void WatcherLogic() => GetAllPartions();

        public void NavigateTo(object detailView)
        {
            DetailView = detailView;
        }

        private void GoBack()
        {
            if (SelectedDirectory == null) return;
            var parent = Directory.GetParent(SelectedDirectory.Path);
            if(parent != null)
            {
                SelectedDirectory = new DirectoryItemViewModel(Directory.GetParent(SelectedDirectory.Path).FullName);
                OnPropertyChanged(nameof(SelectedDirectory));
            }
        }

        private void SelectAll()
        {
            if(SelectedPartition != null && SelectedDirectory == null)
            {
                foreach (var item in SelectedPartition.Items)
                {
                    item.IsSelected = true;
                    OnPropertyChanged();
                }
            }
            else if (SelectedDirectory != null)
                foreach (var item in SelectedDirectory.SubItemViewModels)
                {
                    item.IsSelected = true;
                    OnPropertyChanged();
                }
        }

        private void UnselectAll()
        {
            if (SelectedPartition != null && SelectedDirectory == null)
                foreach (var item in SelectedPartition.Items)
                {
                    item.IsSelected = false;
                    OnPropertyChanged();
                }

            else if (SelectedDirectory != null)
                foreach (var item in SelectedDirectory.SubItemViewModels)
                {
                    item.IsSelected = false;
                    OnPropertyChanged();
                }
            OnPropertyChanged();
        }

        /// <summary>
        /// To get all files from the seletected directories and add these files into the list SelectedFiles
        /// </summary>
        private void GetAllFilesFromSelectedItems()
        {
            SelectedFiles.Clear();
            foreach (var item in SelectedPartition.Items)
            {
                if(item.IsSelected)
                {
                    if(item is FileItemViewModel file)
                    {
                        SelectedFiles.Add(file);
                    }
                    else if(item is DirectoryItemViewModel directory)
                    {
                        var files = directory.GetAllFiles(directory.Path);
                        files.ForEach(f => f.IsSelected = true);
                        SelectedFiles.AddRange(files);
                    }
                }
            }
        }
    }
}

