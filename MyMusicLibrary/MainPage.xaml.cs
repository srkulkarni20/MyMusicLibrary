using MyMusicLibrary.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using Windows.AI.MachineLearning;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyMusicLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Song> Songs;

        private ObservableCollection<Album> Albums;

        private ObservableCollection<Playlist> Playlists;

        public string playlist_tobe_updated;

        public MainPage()
        {
            this.InitializeComponent();

            Songs = new ObservableCollection<Song>();
            Albums = new ObservableCollection<Album>();
            Playlists = new ObservableCollection<Playlist>();

            SongManager.getallSongs(Songs);
            SongManager.getallAlbums(Albums);
            BackButton.Visibility = Visibility.Collapsed;
        }



        //This method displays songs of the album clicked in the library
        private void MyAlbumView_ItemClick(object sender, ItemClickEventArgs e)
        {
            BackButton.Visibility = Visibility.Collapsed;
            MyAlbumView.Header = null;
            Album album = (Album)e.ClickedItem;
            setVisibility_gridview();
            MySongView_display(album);
            MySongView.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;

        }


        //This method populates and displays songs
        private void MySongView_display(Album album_displayed)
        {
            ObservableCollection<Album> album = new ObservableCollection<Album>();
            ObservableCollection<Song> filtered_songs = new ObservableCollection<Song>();

            album.Add(album_displayed);
            SongManager.getsongsbyCategory(filtered_songs, album_displayed.Category);

            MyAlbumView.ItemsSource = album;
            MySongView.ItemsSource = filtered_songs;
            MySongView.SelectionMode = ListViewSelectionMode.None;
            setVisibility_gridview();
            MyAlbumView.Visibility = Visibility.Visible;
            MySongView.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;

        }


        //This method displays albums and songs in home page
        private void AppBarButton_HomeClick(object sender, RoutedEventArgs e)
        {
            MyAlbumView.ItemsSource = Albums;
            MyAlbumView.Header = "ALL ALBUMS";
            MySongView.SelectionMode = ListViewSelectionMode.None;
            SongManager.getallSongs(Songs);
            MySongView.ItemsSource = Songs;
            setVisibility_gridview();
            MySongView.Visibility = Visibility.Visible;
            MyAlbumView.Visibility = Visibility.Visible;
            
           
           
        }

        //This method Creates playlist and updates the playlist class
        private async void AppBarButton_CreatePlaylistClick(object sender, RoutedEventArgs e)
        {
            ContentDialog_CreatePlaylist Dialog = new ContentDialog_CreatePlaylist();
            var result = await Dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var playlist_name = Dialog.Content;
                var playlist_cover = Dialog.PickedStorageImagefile;

                var image = new BitmapImage();
                bool duplicate = SongManager.Check_for_duplicate_playlist(playlist_name.ToString());
                if (duplicate)
                {
                    Display_Dialog(playlist_name.ToString(), "Playlist Already Exists!!");

                }
                else
                {
                    if (Dialog.PickedStorageImagefile != null)
                    {
                        var stream = await playlist_cover.OpenAsync(Windows.Storage.FileAccessMode.Read);
                        image.SetSource(stream);
                    }

                    SongManager.create_playlist(playlist_name.ToString(), image, Playlists);
                    AppBarButton Playlist_btn = new AppBarButton();
                    Playlist_btn.Name = "Playlist";
                    Playlist_btn.Content = playlist_name;
                    Playlist_btn.Width = 60;
                    Playlist_btn.Height = 40;
                    Playlist_btn.HorizontalAlignment = HorizontalAlignment.Center;
                    Playlist_btn.VerticalAlignment = VerticalAlignment.Bottom;
                    MyStackPanel.Children.Add(Playlist_btn);
                    Playlist_btn.Click += Playlist_btn_Click;
                }
            }
            else if (result == ContentDialogResult.Secondary)
            {
                Dialog.Hide();
            }


        }

        //Displays Playlist details
        private void Playlist_btn_Click(object sender, RoutedEventArgs e)
        {

            var clicked_playlist = sender as Button;
            ObservableCollection<Playlist> playlist = new ObservableCollection<Playlist>();
            SongManager.getPlaylistdetails(clicked_playlist.Content.ToString(), playlist);
            MyPlaylistView.ItemsSource = playlist;
            MyPlaylistView.Header = null;
            playlist_tobe_updated = clicked_playlist.Content.ToString();
            MySongView.SelectionMode = ListViewSelectionMode.Multiple;

            Display_PlayList_Details();
        }

        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {

            string text_search = args.QueryText;
            ObservableCollection<Song> searched_songs = new ObservableCollection<Song>();
            MySongView.SelectionMode = ListViewSelectionMode.None;
            SongManager.Search_Songs(text_search, searched_songs);
            if (searched_songs.Count > 0)
            {
                MySongView.ItemsSource = searched_songs;
                setVisibility_gridview();
                MySongView.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Visible;
            }
            else
            {
                Display_Dialog(args.QueryText, "Songs not found");
            }


        }

        //Song Selection to add songs
        private void MySongView_SelectionChanged()
        {
            Song Add_Song;

            foreach (var item in MySongView.SelectedItems)
            {
                var song = item as Song;
                Add_Song = new Song(song.Title, song.Category, song.Duration);
                SongManager.AddSong_Playlist(playlist_tobe_updated, Add_Song);
            }

            Display_PlayList_Details();

        }


        //Adding songs to the playlist
        private void AddSongs_Click(object sender, RoutedEventArgs e)
        {
            MySongView_SelectionChanged();

        }

        private void DeleteSongs_Click(object sender, RoutedEventArgs e)
        {
            MySongPlaylistView_SelectionChanged();
        }

        private void MySongPlaylistView_SelectionChanged()
        {

            Song del_Song;
            foreach (var item in MySongPlaylistView.SelectedItems)
            {
                var song = item as Song;
                del_Song = new Song(song.Title, song.Category, song.Duration);
                SongManager.DeleteSong_Playlist(playlist_tobe_updated, del_Song);
            }

            Display_PlayList_Details();
        }



        //Displays playlist details
        private void Display_PlayList_Details()
        {
            ObservableCollection<Playlist> playlist = new ObservableCollection<Playlist>();
            ObservableCollection<Song> playlist_songs = new ObservableCollection<Song>();
            ObservableCollection<Song> updated_song_list = new ObservableCollection<Song>();

            SongManager.getplaylistdisplaydetails(playlist_tobe_updated, playlist, playlist_songs, updated_song_list);

            MyPlaylistView.ItemsSource = playlist;
            setVisibility_gridview();
            MySongPlaylistView.ItemsSource = playlist_songs;
            MySongPlaylistView.Visibility = Visibility.Visible;
            if (playlist_songs.Count > 0)
            {
                MySongPlaylistView.Visibility = Visibility.Visible;
            }
           
            MyPlaylistView.Visibility = Visibility.Visible;         
            MySongView.ItemsSource = updated_song_list;
            MySongView.Visibility = Visibility.Visible;           
            BackButton.Visibility = Visibility.Visible;
        }


        //This method retrieves the user information from the user

        private async void UserInfo_Click(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<Windows.System.User> users = await Windows.System.User.FindAllAsync();

            var current = users.Where(p => p.AuthenticationStatus == Windows.System.UserAuthenticationStatus.LocallyAuthenticated &&
                                        p.Type == Windows.System.UserType.LocalUser).FirstOrDefault();

            // user may have username
            var data = await current.GetPropertyAsync(Windows.System.KnownUserProperties.AccountName);
            string displayName = (string)data;
            if(displayName==null)
            {
                Display_Dialog(null, "User Information Not Accesible");
            }

            Display_Dialog(displayName, "User Information");

        }



        private async void Display_Dialog(String name, string Title)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = Title,
                Content = name,
                PrimaryButtonText = "Close",

            };
            await dialog.ShowAsync();
        }

      
        private void DisplayPlayLists_Click(object sender, RoutedEventArgs e)
        {

            MyListView.ItemsSource = Playlists;
            setVisibility_gridview();
            MySongView.SelectionMode = ListViewSelectionMode.Multiple;
            MyListView.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
        }

        private void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var playlist = e.ClickedItem as Playlist;
            playlist_tobe_updated = playlist.Name;
            Display_PlayList_Details();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton_HomeClick(null, e);
        }


        private void setVisibility_gridview()
        {
            MyAlbumView.Visibility = Visibility.Collapsed;
            MySongView.Visibility = Visibility.Collapsed;
            MyPlaylistView.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;
            MyListView.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;
            MySongPlaylistView.Visibility = Visibility.Collapsed;


        }

       
    }

}






       