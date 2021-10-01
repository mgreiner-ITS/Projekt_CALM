using BusinessLogic;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSelector.ViewModel
{
    public class ItemViewModelConverter
    {
        public FileItem Convert(FileItemViewModel fileItemViewModel)
        {
            FileItem fileItem = new FileItem()
            {
                Name = fileItemViewModel.Name,
                Content = fileItemViewModel.Content,
                Path = fileItemViewModel.Path,
                Partition = fileItemViewModel.Partition,
                Type = fileItemViewModel.Type,
                LastModified = fileItemViewModel.LastModified
            };

            return fileItem;
        }

        public FileItemViewModel ConvertToFileItemViewModel(FileItem fileItem)
        {
            FileItemViewModel fileItemViewModel = new FileItemViewModel
            {
                Name = fileItem.Name,
                Content = fileItem.Content,
                Path = fileItem.Path,
                Partition = fileItem.Partition,
                Type = fileItem.Type,
                LastModified = fileItem.LastModified
            };

            return fileItemViewModel;
        }
    }
}
