using BusinessLogic;
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
        private string _selectedPartition;
        public ListBox selectionList;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Partitions { get; set; } = new ObservableCollection<string>();
      

        public ObservableCollection<string> FolderandFileOC { get; set; } = new ObservableCollection<string>();



        public ICommand WindowLoadedCommand { get; set; }

        public DataSeletorViewModel()
        {
            _fileFinder = new FileFinder();
            GetAllPartions();
        }

        private void GetAllPartions()
        {
            var partitions = _fileFinder.FindPartitions();
            Partitions.Clear();
            foreach (var item in _fileFinder.FindPartitions())
            {
                PartitionViewModel partitionViewModel = new PartitionViewModel(item);

                Partitions.Add(partitionViewModel.Path);

            }



        }

        public string SelectedPartition
        {
            get { return _selectedPartition; }
            set
            {
                _selectedPartition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedPartition"));
                ShowSelectedPath(_selectedPartition);
            }
        }

        private void ShowSelectedPath(string selectedPartition) ///alle Verzeichnis wurden angezeigt
        {
            var item = _fileFinder.FindFiles(_selectedPartition);
           
            List<CheckBox> list = new List<CheckBox>();
            list.Clear();
            

            foreach (var file in item.directories)
            {CheckBox cb = new CheckBox();
            TextBlock tb = new TextBlock();
         

                tb.Text = Path.GetFileName(file);
                cb.Content = tb;
                list.Add(cb);
            }

            foreach (var file in item.files)
            {
                if (file == ".txt" || file == ".pdf")
                {
                    CheckBox cb = new CheckBox();
                    TextBlock tb = new TextBlock();

                    tb.Text = file;
                    cb.Content = tb;
                    list.Add(cb);
                }
                else
                {
                    CheckBox cb = new CheckBox();
                    TextBlock tb = new TextBlock();
                    tb.Text = file;
                    tb.Foreground = System.Windows.Media.Brushes.Gray;
                    cb.Content = tb;  
                    list.Add(cb);
                }
             
            }

            selectionList.ItemsSource = list;
        }
    }
}
