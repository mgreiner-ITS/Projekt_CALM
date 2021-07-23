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
    public partial class MainWindow : Window
    {
        DataSeletorViewModel _dataSeletorViewModel;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _dataSeletorViewModel;

            _dataSeletorViewModel = new DataSeletorViewModel();
            
            //_dataSeletorViewModel.selectionList = (ListBox)this.FindName("SelectionList");
           
        }

        //private void SelectionList_SelectionChanged(object sender, SelectionChangedEventArgs e) 
    }
}
