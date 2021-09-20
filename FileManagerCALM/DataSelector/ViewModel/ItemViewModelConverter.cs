using BusinessLogic;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSelector.ViewModel
{
    class ItemViewModelConverter
    {
        public FileItem convert(FileItemViewModel fileItemViewModel)
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
    }
}
