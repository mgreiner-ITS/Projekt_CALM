﻿using BusinessLogic;
using BusinessLogic.Management;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSelector.ViewModel
{
    public class PartitionViewModel
    {
        private readonly FileFinder _fileFinder;

        public string Name { get; set; }
        public DriveType DriveType { get; set; }
        public bool IsReady { get; set; }
        public ObservableCollection<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();
        public PartitionViewModel(DriveInfo driveInfo)
        {
            _fileFinder = new FileFinder();
            Name = driveInfo.Name;
            DriveType = driveInfo.DriveType;
            IsReady = driveInfo.IsReady;
            InitializeItems();
            //CreateSampleItems();
        }

        private void InitializeItems()
        {
            var items = _fileFinder.FindFiles(new DriveInfo(Name));

            foreach (var directoryName in items.directories)
            {
                DirectoryItemViewModel directoryViewModel = new DirectoryItemViewModel { Name = directoryName.Name };
                Items.Add(directoryViewModel);
            }

            foreach (var fileName in items.files)
            {
                FileItemViewModel fileItemViewModel = new FileItemViewModel { Name = fileName.Name };
                Items.Add(fileItemViewModel);
            }

        }

        //private void CreateSampleItems()
        //{
        //    var folder1 = new DirectoryItem { Name = "Folder1" };
        //    var folder2 = new DirectoryItem { Name = "Folder2" };
        //    var folder3 = new DirectoryItem { Name = "Folder3" };

        //    var folder1a = new DirectoryItem { Name = "Folder1a" };
        //    var folder1b = new DirectoryItem { Name = "Folder1b" };
        //    folder1.SubDirectories.Add(folder1a);
        //    folder1.SubDirectories.Add(folder1b);

        //    var folder2a = new DirectoryItem { Name = "Folder2a" };
        //    folder2a.SubDirectories.Add(new DirectoryItem { Name = "Folder2a_1" });
        //    folder2a.SubDirectories.Add(new DirectoryItem { Name = "Folder2a_2" });

        //    var folder2b = new DirectoryItem { Name = "Folder2b" };
        //    folder2.SubDirectories.Add(folder2a);
        //    folder2.SubDirectories.Add(folder2b);

        //    var folder3a = new DirectoryItem { Name = "Folder3a" };
        //    var folder3b = new DirectoryItem { Name = "Folder3b" };
        //    folder3.SubDirectories.Add(folder3a);
        //    folder3.SubDirectories.Add(folder3b);

        //    Items.Add(folder1);
        //    Items.Add(folder2);
        //    Items.Add(folder3);
        //}

    }
}
