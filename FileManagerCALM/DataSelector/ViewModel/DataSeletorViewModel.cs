using BusinessLogic;
using BusinessLogic.Management;
using CommandHelper;
using DataSelector.View;
using Models;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ICommand WindowLoadedCommand { get; set; }
        public RelayCommand SyncCommand { get; set; }
        public RelayCommand CancelSyncCommand { get; set; }

        public DataSeletorViewModel()
        {
            _uploadManagement = new UploadManagement();
            ShowSearchWindowCmd = new DelegateCommand(ShowMethod);
            _fileFinder = new FileFinder();

            SyncCommand = new RelayCommand(unused => Sync());
            CancelSyncCommand = new RelayCommand(unused => CancelSync());

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

        public ICommand ShowSearchWindowCmd { get; set; }

        private void GetAllPartions()
        {
            Partitions.Clear();
            foreach (var item in _fileFinder.FindPartitions())
            {
                PartitionViewModel partitionViewModel = new PartitionViewModel(item);


                Partitions.Add(partitionViewModel);

            }
        }


        public PartitionViewModel SelectedPartition
        {
            get { return _selectedPartition; }
            set
            {
                _selectedPartition = value;
                OnPropertyChanged();
            }
        }

        public DirectoryItemViewModel SelectedDirectory
        {
            get { return _selectedDirectory; }
            set
            {
                _selectedDirectory = value;
                OnPropertyChanged();
            }
        }

        //     selectionList.ItemsSource = list;
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

    }
}

