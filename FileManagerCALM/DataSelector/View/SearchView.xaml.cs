using DataSelector.ViewModel;
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
using System.Windows.Shapes;

namespace DataSelector.View
{
    /// <summary>
    /// Interaktionslogik für SearchView.xaml
    /// </summary>
    public partial class SearchView : Window
    {
       // bool isOpen = false;

        public SearchView()
        {
            InitializeComponent();
            var vm = new SearchViewModel();
            DataContext = vm;


        }

        //public new void Show()
        //{
        //    //if (!isOpen)
        //    //{
        //       base.Show();
        //    //}
        //   // isOpen = true;
        //}

        //public new void Close() 
        //{
        //    base.Close();
        //    isOpen = false;
        //}

    }
}
