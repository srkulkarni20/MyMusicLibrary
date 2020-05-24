using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MyMusicLibrary.Model
{
    public enum PlaylistCover
    {
        Favorite,
        Kids,
        Party,
        Romantic
    }

         
   public class Playlist
   {
       public string Name { get; set; }
       public int NumOfSongs { get; set; } 
       public List<Song> PlaylistSongs = new List<Song>();           
       public BitmapImage ImageSource;


      public Playlist(string name, BitmapImage cover)
      {
          Name = name;
          ImageSource = cover;
          NumOfSongs = 0;

      }
     
        //Adding song in playlist
      public void AddSong(Song song)
      {
          this.NumOfSongs++;
          this.PlaylistSongs.Add(new Song(song.Title, song.Category, song.Duration)) ;
      }

            //Deleting the song from playlist
      public void DeleteSong(Song song)
      {
           this.PlaylistSongs.Remove(song);
           this.NumOfSongs--;
      }

            
        }
   }

