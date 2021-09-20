using BusinessLogic;
using BusinessLogic.Management;
using CommandHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Management;
using System.Windows.Controls;
using System.Windows.Input;


namespace DataSelector.ViewModel
{
    public class DataSeletorViewModel : INotifyPropertyChanged
    {
        private FileFinder _fileFinder;
        private PartitionViewModel _selectedPartition;
        private DirectoryItemViewModel _selectedDirectory;
        public ListBox selectionList;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PartitionViewModel> Partitions { get; set; } = new ObservableCollection<PartitionViewModel>();

        public ObservableCollection<TreeViewItem> SelectionOverview { get; set; } = new ObservableCollection<TreeViewItem>();


        public ObservableCollection<string> FolderandFileOC { get; set; } = new ObservableCollection<string>();



        public ICommand WindowLoadedCommand { get; set; }

        public DataSeletorViewModel()
        {
            _fileFinder = new FileFinder();

            GetAllPartions();

            SelectionOverview.Add(new TreeViewItem() { Header = "Test" }
                                                       );
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


        public PartitionViewModel SelectedPartition
        {
            get { return _selectedPartition; }
            set
            {
                _selectedPartition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedPartition"));
            }
        }

        public DirectoryItemViewModel SelectedDirectory
        {
            get { return _selectedDirectory; }
            set
            {
                _selectedDirectory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedDirectory"));
            }
        }

        //     selectionList.ItemsSource = list;

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

