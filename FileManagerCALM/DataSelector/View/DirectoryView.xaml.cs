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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataSelector.View
{
    /// <summary>
    /// Interaction logic for DirectoryView.xaml
    /// </summary>
    public partial class DirectoryView : UserControl
    {
        public DirectoryView()
        {
            InitializeComponent();
        }

        private void DirectoryListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var directoryItemViewModel = ((FrameworkElement)e.OriginalSource).DataContext as DirectoryItemViewModel;
            if(directoryItemViewModel != null)
            {
                var parent = this.Parent as FrameworkElement;
                while(!(parent is Window))
                {
                    parent = parent.Parent as FrameworkElement;
                }

                DataSeletorViewModel dataSelectorViewModel = parent.DataContext as DataSeletorViewModel;
                dataSelectorViewModel.ParentDirectory = dataSelectorViewModel.SelectedDirectory;
                var subFolderView = new SubfolderView();
                dataSelectorViewModel.NavigateTo(subFolderView);
            }
        }
    }
}
