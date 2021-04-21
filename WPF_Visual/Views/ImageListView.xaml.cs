using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Visual.Views
{
    /// <summary>
    /// Interaction logic for ImageView.xaml
    /// </summary>
    public partial class ImageListView : UserControl
    {
        public int SelectedListIndex { get; private set; }

        public ImageListView()
        {
            InitializeComponent();
            this.ImageDetailsGrid.Visibility = Visibility.Collapsed;
            this.DetailsButton.IsEnabled = false;
            this.ReturnButton.IsEnabled = false;
            this.RemoveButton.IsEnabled = false;
            ((ViewModels.ImageListViewModel)this.FindResource("ViewModel")).Images.CollectionChanged += Images_CollectionChanged;
        }

        private void Images_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Models.ImageListModel> model = (ObservableCollection<Models.ImageListModel>)sender;
            if (model.Count > 0 && this.ImagesListView.SelectedIndex == -1 && this.ImagesListView.Items.Count > 0)
            {
                this.ImagesListView.SelectedItem = 0;
            }
            else if (model.Count == 0)
            {
                this.ImagesListView.SelectedIndex = -1;
                if (this.ImagesListView.Items.Count > 0)
                {
                    this.ImagesListView.Items.Clear();
                }
                this.DetailsButton.IsEnabled = false;
                this.ReturnButton.IsEnabled = false;
                this.RemoveButton.IsEnabled = false;
            }
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ImagesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImagesListView.Items.Count > 0)
            {
                this.DetailsButton.IsEnabled = true;
                this.RemoveButton.IsEnabled = true;
            }
            SelectedListIndex = ImagesListView.SelectedIndex;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.ImagesListView.Visibility = Visibility.Visible;
            this.ImageDetailsGrid.Visibility = Visibility.Collapsed;
            this.DetailsButton.IsEnabled = true;
            this.ReturnButton.IsEnabled = false;
            ImagesListView.SelectedIndex = SelectedListIndex;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModels.ImageListViewModel viewModel = (ViewModels.ImageListViewModel)this.FindResource("ViewModel");
            viewModel.Images.Remove(viewModel.Images[ImagesListView.SelectedIndex]);
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            this.ImagesListView.Visibility = Visibility.Collapsed;
            this.ImageDetailsGrid.Visibility = Visibility.Visible;
            this.DetailsButton.IsEnabled = false;
            this.ReturnButton.IsEnabled = true;
        }

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}