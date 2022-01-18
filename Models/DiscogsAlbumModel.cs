namespace SpotifyAlbumCreator.Models
{
    public class DiscogsAlbumModel
    {
        public int ID {get; set;}
        public string Title {get; set;}
        public string Year {get; set;}
        public string ThumbnailUrl {get; set;}        

        public ICollection<String> Songs {get; set;}

    }
}