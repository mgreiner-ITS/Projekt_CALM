using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessLogic;
using DataSelector.ViewModel;

namespace DataSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DataSelectorView : Window
    {
        DataSeletorViewModel _dataSeletorViewModel;
        public DataSelectorView()
        {
            InitializeComponent();

            var vm = new DataSeletorViewModel();
            DataContext = vm;
            //_dataSeletorViewModel = new DataSeletorViewModel();
            //DataContext = _dataSeletorViewModel;

            //_dataSeletorViewModel.selectionList = (ListBox)this.FindName("SelectionList");

        }

        
    }
}
