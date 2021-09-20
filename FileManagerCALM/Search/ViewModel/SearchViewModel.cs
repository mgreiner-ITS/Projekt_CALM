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
            BusinessLogic.Management.SearchManagement _searchItem = new BusinessLogic.Management.SearchManagement();
            SearchDataCommand = new RelayCommand(c => SearchData());
        }
        
        private void SearchData()
        {
            string a = Text;
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
