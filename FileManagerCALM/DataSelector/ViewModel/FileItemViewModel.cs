using BusinessLogic;
using Models;
using System;
using System.IO;

namespace DataSelector.ViewModel
{
    public class FileItemViewModel : ItemViewModel
    {
        private const string FileIcon = "pack://application:,,,/Resources/file.png";

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

        public FileItemViewModel()
        {
            IconPath = FileIcon;
        }

        public FileItemViewModel(FileInfo fileInfo)
        {
            Path = fileInfo.FullName;
            Name = fileInfo.Name;
            LastModified = fileInfo.LastWriteTime;
            IconPath = FileIcon;
            Partition = Path.Substring(0, 1);
        }

    }
}
