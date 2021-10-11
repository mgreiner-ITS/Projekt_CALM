using CommandHelper;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BusinessLogic.Management;
using Microsoft.Xaml.Behaviors.Core;
using BusinessLogic.Management.FileManagement;

namespace DataSelector.ViewModel
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        FileOpener _fileOpener;

        BusinessLogic.Management.SearchManagement _searchItem;
        List<FileItem> listItems;
        public ObservableCollection<FileItem> fileItems { get; set; } = new ObservableCollection<FileItem>();
        public ICommand SearchDataCommand { get; set; }
       
        private ICommand _enterCmd;
        public ICommand EnterCmd
        {
            get
            {
                return _enterCmd
                    ?? (_enterCmd = new ActionCommand(() =>
                    {
                        SearchData();
                    }));
            }
        }
        public ICommand DoubleClickCommand { get; set; }
        //public ICollectionView CollectionView { get; set; }
        FileItem _selectedItem;

        public SearchViewModel()
        {
            _searchItem = new BusinessLogic.Management.SearchManagement();

          
            SearchDataCommand = new RelayCommand(c => SearchData());
          
            DoubleClickCommand = new RelayCommand(c => ShowFile());

        }
        public FileItem SelectedItem // methode
        {
            get { return _selectedItem; }

            set
            {
                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedItem")); // Selected muss Property Changed
            }
        }

        private void ShowFile()
        {
            _fileOpener = new FileOpener(SelectedItem);

        }
  

        private void SearchData()
        {
            fileItems.Clear();
            listItems = _searchItem.SearchText(_text);
            foreach (var item in listItems)
            {
                fileItems.Add(item);
            }
        }

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private string _text;
        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                if (value != this._text)
                {
                    this._text = value;
                    OnPropertyChanged("Text");
                }
                //OnPropertyChanged("Test");
            }
        }

        private void GetFileItemByText(List<FileItem> list)
        {
            foreach (var item in list)
            {
                fileItems.Add(item);
            }
        }



    }
}
