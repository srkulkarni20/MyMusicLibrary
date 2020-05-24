using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicLibrary.Model
{
    public class Album
    {
        
            public string Name { get; set; }
            public string AlbumCover { get; set; }
            public int NumOfSongs { get; set; }
            public AlbumCategory Category { get; set; }
            public List<Song> AlbumSongs { get; set; }

            public Album(string name, AlbumCategory albumCategory, List<Song> songs)
            {
                AlbumSongs = new List<Song>();
                Name = name;
                Category = albumCategory;
                AlbumCover = $"/Assets/AlbumCover/{albumCategory}.PNG";
                songs.ForEach(song => AlbumSongs.Add(song));
                
            }
        
    }
}
