using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyMusicLibrary
{


    public sealed partial class ContentDialog_CreatePlaylist : ContentDialog
    {

        public StorageFile PickedStorageImagefile;
        public ContentDialog_CreatePlaylist()
        {
            this.InitializeComponent();
        }

        public void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Content = Playlist_Create.Text;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Content = "Cancel";
        }

        private async void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            var file = await picker.PickSingleFileAsync();
            PickedStorageImagefile = file;
            var image = new BitmapImage();
            if (PickedStorageImagefile != null)
            {
                var stream = await PickedStorageImagefile.OpenAsync(Windows.Storage.FileAccessMode.Read);
               
                await image.SetSourceAsync(stream);
                var imagebrush = new ImageBrush();
                imagebrush.ImageSource = image;
                AddPhoto.Background = imagebrush;
                AddPhoto.Content = "";
            }
        }
    }
}