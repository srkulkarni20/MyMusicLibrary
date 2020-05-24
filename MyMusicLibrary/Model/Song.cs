using System;
using Windows.Media.Core;

namespace MyMusicLibrary.Model
{


    public enum AlbumCategory
    {
        Cinematic,
        Folk,
        Jazz,
        Pop,
        Rock
    }
    public class Song
    {
        public string Title { get; set; }
        public AlbumCategory Category { get; set; }
        public double Duration { get; set; }      
        public string ImageFile { get; set; }
        public string AudioFile { get; set; }
        public MediaSource source;



        // create a song global list of all songs present in a music library, for searching feature
        // public static List<Song> SongsList = new List<Song>();


        public Song(string title, AlbumCategory category, double duration)
        {

            Title = title;
            Category = category;
            Duration = duration;          
            ImageFile = $"/Assets/Images/{Category}/{Title}.png";
            AudioFile = $"ms-appx:///Assets/Audio/{Category}/{Title}.mp3";
            Uri uri = new Uri(AudioFile);
            source = MediaSource.CreateFromUri(uri);
        }

    }


}