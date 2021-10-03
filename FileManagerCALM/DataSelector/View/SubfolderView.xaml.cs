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
    /// Interaction logic for SubfolderView.xaml
    /// </summary>
    public partial class SubfolderView : UserControl
    {
        public SubfolderView()
        {
            InitializeComponent();
        }

        private void SubFolderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var subfolderListView = (ListView)sender;
            var directoryItemViewModel = ((FrameworkElement)e.OriginalSource).DataContext as DirectoryItemViewModel;
            if (directoryItemViewModel != null)
            {
                var parent = this.Parent as FrameworkElement;
                while (!(parent is Window))
                {
                    parent = parent.Parent as FrameworkElement;
                }

                DataSeletorViewModel dataSelectorViewModel = parent.DataContext as DataSeletorViewModel;
                dataSelectorViewModel.SelectedDirectory = subfolderListView.SelectedItem as DirectoryItemViewModel;
                var subFolderView = new SubfolderView();
                dataSelectorViewModel.NavigateTo(subFolderView);
            }
        }
    }
}
