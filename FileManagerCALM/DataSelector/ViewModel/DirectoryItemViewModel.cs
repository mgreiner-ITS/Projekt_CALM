using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataSelector.ViewModel
{
    public class DirectoryItemViewModel : ItemViewModel
    {
        public ObservableCollection<ItemViewModel> SubItemViewModels { get; set; } = new ObservableCollection<ItemViewModel>();
    }
}
