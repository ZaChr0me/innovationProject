using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using WPF_Visual.Models;

namespace WPF_Visual.ViewModels
{
    public class ImageListViewModel
    {
        public ObservableCollection<ImageListModel> Images { get; set; }

        public ImageListViewModel()
        {
            Images = new ObservableCollection<ImageListModel>();
        }
    }
}