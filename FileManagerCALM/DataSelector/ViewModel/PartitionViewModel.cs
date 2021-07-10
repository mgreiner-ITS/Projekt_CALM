using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSelector.ViewModel
{
    public class PartitionViewModel
    {
        public string Path { get; set; }
        public DriveType DriveType { get; set; }
        public bool IsReady { get; set; }
        public ObservableCollection<object> Items { get; set; } = new ObservableCollection<Object>();
        public PartitionViewModel( DriveInfo driveInfo)
        {
            Path = driveInfo.Name;
            DriveType = driveInfo.DriveType;
            IsReady = driveInfo.IsReady;

        }
    }
}
