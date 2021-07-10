using BusinessLogic;
using CommandHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataSelector.ViewModel
{
    public class DataSeletorViewModel
    {
        private FileFinder _fileFinder;
        public ObservableCollection<PartitionViewModel> Partitions { get; set; } = new ObservableCollection<PartitionViewModel>();

        public ICommand WindowLoadedCommand { get; set; }

        public DataSeletorViewModel()
        {
            _fileFinder = new FileFinder();
        }

        private void GetAllPartions()
        {
            var partitions = _fileFinder.FindPartitions();
            Partitions.Clear();
            foreach (var item in _fileFinder.FindPartitions())
            {
                PartitionViewModel partitionViewModel = new PartitionViewModel(item);

                Partitions.Add(partitionViewModel);

            }
        }

    }
}
