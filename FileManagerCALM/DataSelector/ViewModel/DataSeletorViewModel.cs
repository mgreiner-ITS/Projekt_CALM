using BusinessLogic;
using BusinessLogic.Management;
using CommandHelper;
using DataSelector.View;
using Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace DataSelector.ViewModel
{
    public class DataSeletorViewModel : ViewModelBase
    {
        private readonly FileFinder _fileFinder;
        private readonly UploadManagement _uploadManagement;
        private readonly MonitoringManagement _watcherManagement;
        private readonly SynchronizationContext _synchronizationContext;

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
            _synchronizationContext = SynchronizationContext.Current;

            _uploadManagement = new UploadManagement();
            _watcherManagement = new MonitoringManagement();

            ShowSearchWindowCmd = new DelegateCommand(ShowMethod);
            _fileFinder = new FileFinder();

            SyncCommand = new RelayCommand(unused => Sync());
            CancelSyncCommand = new RelayCommand(unused => CancelSync());
            GoBackCommand = new RelayCommand(unused => GoBack());
            SelectAllCommand = new RelayCommand(unused => SelectAll());
            UnselectAllCommand = new RelayCommand(unused => UnselectAll());

            MonitorUsbInputs();
            GetAllPartions();
            SetWatcherForFiles();

            SelectionOverview.Add(new TreeViewItem() { Header = "Test" }
                                                       );
        }



        private void Sync()
        {
            GetAllFilesFromSelectedItems();
            List<ItemViewModel> itemViewModels = SelectedItemViewModels.ToList();
            
            cancelledSync = false;

            double numberOfItems = SelectedFiles.Count();
            double numberOfProcessedItems = 0;
            ProgressBarProgress = 0;

            //var sync = SynchronizationContext.Current;

            Task.Run(() =>
            {
                foreach (FileItemViewModel currentFileViewModel in SelectedFiles)
                {
                    if (cancelledSync)
                        break;

                    ItemViewModelConverter converter = new ItemViewModelConverter();
                    FileItem currentFileItem = converter.Convert(currentFileViewModel);

                    _uploadManagement.UploadFile(currentFileItem);

                    numberOfProcessedItems++;
                    ProgressBarProgress = (int)((numberOfProcessedItems / numberOfItems) * 100);
                }
            });
        }

        private void CancelSync()
        {
            cancelledSync = true;
        }

        SearchView objSearchView = null;
        private void ShowMethod()
        {/// Abfragen ob, das Fenst SearchUi schon angezeigt ist, wenn ja dann sollte keine neue UI anzeigen - Nguyen
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
                if (value != null)
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
                if (value != null)
                {
                    _selectedDirectory.InitializeSubItems();
                }
                OnPropertyChanged();
            }
        }

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
            _synchronizationContext.Send(unused => { Partitions.Clear(); }, null);

            foreach (var item in _fileFinder.FindPartitions())
            {
                PartitionViewModel partitionViewModel = new PartitionViewModel(item);
                _synchronizationContext.Send(unused => { Partitions.Add(partitionViewModel); }, null);
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
            if (parent != null)
            {
                SelectedDirectory = new DirectoryItemViewModel(Directory.GetParent(SelectedDirectory.Path).FullName);
                OnPropertyChanged(nameof(SelectedDirectory));
            }
        }

        private void SelectAll()
        {
            if (SelectedPartition != null && SelectedDirectory == null)
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
            if (SelectedDirectory != null)
            {
                foreach (var item in SelectedDirectory.SubItemViewModels)
                {
                    if (item is FileItemViewModel file && file.IsSelected)
                        SelectedFiles.Add(file);
                    else if (item is DirectoryItemViewModel directory && directory.IsSelected)
                    {
                        var files = directory.GetAllFiles(directory.Path);
                        files.ForEach(f => f.IsSelected = true);
                        SelectedFiles.AddRange(directory.GetAllFiles(directory.Path));
                    }
                }
            }
            else
            {
                foreach (var item in SelectedPartition.Items)
                {
                    if (item is FileItemViewModel file && file.IsSelected)
                        SelectedFiles.Add(file);
                    else if (item is DirectoryItemViewModel directory && directory.IsSelected)
                    {
                        var files = directory.GetAllFiles(directory.Path);
                        files.ForEach(f => f.IsSelected = true);
                        SelectedFiles.AddRange(files);
                    }
                }
            }
        }

        private void SetWatcherForFiles()
        {
            _watcherManagement.InitialzeWatcher();
        }
    }
}