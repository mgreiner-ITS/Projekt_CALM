using BusinessLogic;
using CommandHelper;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;


namespace DataSelector.ViewModel
{
    public class DataSeletorViewModel : INotifyPropertyChanged
    {
        private FileFinder _fileFinder;
        private PartitionViewModel _selectedPartition;
        public ListBox selectionList;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PartitionViewModel> Partitions { get; set; } = new ObservableCollection<PartitionViewModel>();

        public ObservableCollection<TreeViewItem> SelectionOverview { get; set; } = new ObservableCollection<TreeViewItem>();


        public ObservableCollection<string> FolderandFileOC { get; set; } = new ObservableCollection<string>();



        public ICommand WindowLoadedCommand { get; set; }

        public DataSeletorViewModel()
        {
            _fileFinder = new FileFinder();

            DB dB = new DB();
            //FileItem FakeItem = new FileItem();
            //FakeItem.Name = "test.txt";
            //FakeItem.Partition = "C";
            //FakeItem.Path =  @"C:\\Users\\winkler\\Downloads\\test.txt";
            //FakeItem.Type = FileType.other;
            //FakeItem.Content = "flfgkgl";
           // dB.InsertData(FakeItem);
            dB.GetFileItems("Test");

            GetAllPartions();

            SelectionOverview.Add(new TreeViewItem() { Header = "Test" }
                                                       );
        }

        private void GetAllPartions()
        {
          //  var partitions = _fileFinder.FindPartitions();
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
                ShowSelectedPath(_selectedPartition.Name);
            }
        }

        private void ShowSelectedPath(string selectedPartition) ///alle Verzeichnis wurden angezeigt
        {
            var item = _fileFinder.FindFiles(_selectedPartition.Name);

            List<CheckBox> list = new List<CheckBox>();
            list.Clear();


            foreach (var file in item.directories)
            { CheckBox cb = new CheckBox();
                TextBlock tb = new TextBlock();


                tb.Text = Path.GetFileName(file);
                cb.Content = tb;
                list.Add(cb);
            }

            //foreach (var file in item.files)
            //{
            //    if (file == ".txt" || file == ".pdf")
            //    {
            //        CheckBox cb = new CheckBox();
            //        TextBlock tb = new TextBlock();

            //        tb.Text = file;
            //        cb.Content = tb;
            //        list.Add(cb);
            //    }
            //    else
            //    {
            //        CheckBox cb = new CheckBox();
            //        TextBlock tb = new TextBlock();
            //        tb.Text = file;
            //        tb.Foreground = System.Windows.Media.Brushes.Gray;
            //        cb.Content = tb;  
            //        list.Add(cb);
            //    }

        }

        //     selectionList.ItemsSource = list;
    }

    }

