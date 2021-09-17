using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSelector.ViewModel
{
    class FileItemViewModel : ItemViewModel
    {
        private long _id;

        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private FileType _type;

        public FileType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        private string _partition;

        public string Partition
        {
            get { return _partition; }
            set
            {
                _partition = value;
                OnPropertyChanged();
            }
        }

        private DateTime _lastModified;

        public DateTime LastModified
        {
            get { return _lastModified; }
            set
            {
                _lastModified = value;
                OnPropertyChanged();
            }
        }
        
    }
}
