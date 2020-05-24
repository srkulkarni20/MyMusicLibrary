using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;

namespace MyMusicLibrary.Model
{
    class SongManager
    {


        public static List<Song> allSongs;
        public static List<Playlist> allplaylists = new List<Playlist>();

        //Method to update collection of songs
        public static void getallSongs(ObservableCollection<Song> songs)
        {
            allSongs = getSongs();
            songs.Clear();
            allSongs.ForEach(song => songs.Add(new Song(song.Title, song.Category, song.Duration)));//lambda expression

        }

        

        //This method populates the songs in the music library
        private static List<Song> getSongs()
        {
            var songs = new List<Song>();
            songs.Add(new Song("betterdays", AlbumCategory.Cinematic, 2.33));
            songs.Add(new Song("epic", AlbumCategory.Cinematic, 2.58));
            songs.Add(new Song("memories", AlbumCategory.Cinematic, 3.50));
            songs.Add(new Song("onceagain", AlbumCategory.Cinematic, 3.51));

            songs.Add(new Song("acousticbreeze", AlbumCategory.Folk, 2.37));
            songs.Add(new Song("buddy", AlbumCategory.Folk, 2.02));
            songs.Add(new Song("cute", AlbumCategory.Folk, 3.14));
            songs.Add(new Song("ukulele", AlbumCategory.Folk, 2.26));

            songs.Add(new Song("allthat", AlbumCategory.Jazz, 2.25));
            songs.Add(new Song("jazzyfrenchy", AlbumCategory.Jazz, 1.44));

            songs.Add(new Song("creativeminds", AlbumCategory.Pop, 2.27));
            songs.Add(new Song("energy", AlbumCategory.Pop, 2.59));
            songs.Add(new Song("hey", AlbumCategory.Pop, 2.51));
            songs.Add(new Song("littleidea", AlbumCategory.Pop, 2.49));

            songs.Add(new Song("anewbeginning", AlbumCategory.Rock, 2.34));
            songs.Add(new Song("happyrock", AlbumCategory.Rock, 1.45));

            return songs;
        }


        //This method populates the Albums in the music library
        public static void getallAlbums(ObservableCollection<Album> Albums)
        {
            var albums = new List<Album>();
            var songs = new List<Song>();


            songs = allSongs.FindAll(song => song.Category == AlbumCategory.Cinematic);
            albums.Add(new Album("Cinematic", AlbumCategory.Cinematic, songs));
            songs.Clear();
            songs = allSongs.FindAll(song => song.Category == AlbumCategory.Folk);
            albums.Add(new Album("Folk", AlbumCategory.Folk, songs));
            songs.Clear();
            songs = allSongs.FindAll(song => song.Category == AlbumCategory.Folk);
            albums.Add(new Album("Jazz", AlbumCategory.Jazz, songs));
            songs.Clear();
            songs = allSongs.FindAll(song => song.Category == AlbumCategory.Jazz);
            albums.Add(new Album("Jazz", AlbumCategory.Pop, songs));
            songs.Clear();
            songs = allSongs.FindAll(song => song.Category == AlbumCategory.Rock);
            albums.Add(new Album("Jazz", AlbumCategory.Rock, songs));
            Albums.Clear();

            albums.ForEach(album => Albums.Add(album));

        }


        //This method helps to filter the somgs based on the album category
        public static void getsongsbyCategory(ObservableCollection<Song> songs, AlbumCategory category)
        {
            var allSongs = getSongs();
            var filteredsongs = allSongs.Where(song => song.Category == category).ToList();
            songs.Clear();
            filteredsongs.ForEach(song => songs.Add(song));

        }


        //This method creates the new playlist in the Music Library.
        public static void create_playlist(string name, BitmapImage image, ObservableCollection<Playlist> playlist)
        {
            Playlist playlist_new = new Playlist(name, image);
            playlist.Add(new Playlist(playlist_new.Name,playlist_new.ImageSource));
            allplaylists.Add(playlist_new);

        }

        //This method retrieves the playlist details based on the name pf the playlist
        public static void getPlaylistdetails(string name, ObservableCollection<Playlist> playlist)
        {
            var filteredplaylist = allplaylists.Find(playlist_disp => playlist_disp.Name == name);
            playlist.Clear();
            playlist.Add(filteredplaylist);
        }


        //This method searches the songs for substring given by the user
        public static void Search_Songs(string name, ObservableCollection<Song> result_songs)
        {
            result_songs.Clear();

            foreach (var item in allSongs)
            {
                if (item.Title.Contains(name))
                    result_songs.Add(new Song(item.Title,item.Category,item.Duration));
            }
        }

        //This method adds songs to the playlist
        public static void AddSong_Playlist(string name, Song song)
        {
            int index = 0;
            for (index=0;index<allplaylists.Count;index++)
            {
                if(allplaylists[index].Name==name)
                {
                    break;
                }
            }
            
            allplaylists[index].AddSong(song);
                   
                    
        }

        //This method used to get all the details of the playlist for displaying to the user
        public static void getplaylistdisplaydetails(string name, ObservableCollection<Playlist> playlist, ObservableCollection<Song> Playlistsongs,ObservableCollection<Song> updated_songs)
        {
            Playlist clickedplaylist = allplaylists.Find(playlist_disp => playlist_disp.Name == name);
            var songs = new List<Song>();
            playlist.Clear();
            playlist.Add(clickedplaylist);
            Playlistsongs.Clear();
            updated_songs.Clear();
            allSongs.ForEach(item => songs.Add(new Song(item.Title, item.Category,item.Duration)));
            if(clickedplaylist.NumOfSongs>0)
            {
                for (int i = 0; i< clickedplaylist.NumOfSongs; i++)
                {
                    Playlistsongs.Add(new Song(clickedplaylist.PlaylistSongs[i].Title, clickedplaylist.PlaylistSongs[i].Category, clickedplaylist.PlaylistSongs[i].Duration));
                    var item =songs.Find(song=>song.Title==clickedplaylist.PlaylistSongs[i].Title);
                    songs.Remove(item);
                       
                }
            }
            songs.ForEach(song => updated_songs.Add(song));
        }


        //This method deletes song from the playlist
        public static void DeleteSong_Playlist(string name, Song song)
        {
            Playlist clickedplaylist = allplaylists.Find(playlist_disp => playlist_disp.Name == name);
            for (int i=0;i<clickedplaylist.NumOfSongs;i++)
            {
                if(clickedplaylist.PlaylistSongs[i].Title==song.Title)
                {
                    clickedplaylist.DeleteSong(clickedplaylist.PlaylistSongs[i]);
                    break;
                }
            }


        }

        //This method checks for duplicate playlist name
        public static bool Check_for_duplicate_playlist(string name)
        {
               foreach(var item in allplaylists)
            {
                if (item.Name == name)
                    return true;
            }
            return false;
        }


        
    }




}
    

