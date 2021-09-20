using CommandHelper;

using System;
using System.ComponentModel;
using System.Windows.Input;

namespace DataSelector.ViewModel
{
   public class SearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        BusinessLogic.Management.SearchManagement _searchItem;
        public ICommand SearchDataCommand { get; set; }
        public SearchViewModel()
        {
            _searchItem = new BusinessLogic.Management.SearchManagement();
            SearchDataCommand = new RelayCommand(c => SearchData());
        }

        private void SearchData()
        {
            _searchItem.SearchText(_text);

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

     



}
}
