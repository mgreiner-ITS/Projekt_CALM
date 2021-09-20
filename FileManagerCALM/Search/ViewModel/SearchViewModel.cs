using BusinessLogic;
using CommandHelper;
using System;
using System.ComponentModel;
using System.Windows.Input;
namespace Search.ViewModel
{
    class SearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SearchDataCommand { get; set; }
        public SearchViewModel()
        {
            SearchModel _searchItem = new SearchModel();
            SearchDataCommand = new RelayCommand(c => SearchData());
        }
        
        private void SearchData()
        {

        }

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private string _Text;
        public string Text
        {
            get
            {
                return this._Text;
            }
            set
            {
                if (value != this._Text)
                {
                    this._Text = value;
                    OnPropertyChanged("Text");
                }
                //OnPropertyChanged("Test");
            }
        }



    }
}
