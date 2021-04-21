using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media.Imaging;

namespace WPF_Visual.Models
{
    public class ImageListModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private BitmapSource picture;

        public BitmapSource Picture
        {
            get { return picture; }
            set { SetField(ref picture, value, "Picture"); }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { SetField(ref description, value, "Description"); }
        }

        private string percentChance;

        public string PercentChance
        {
            get { return percentChance; }
            set { SetField(ref percentChance, value, "PercentChance"); }
        }
    }
}