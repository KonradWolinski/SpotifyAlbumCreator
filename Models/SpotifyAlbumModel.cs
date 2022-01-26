namespace SpotifyAlbumCreator.Models
{
    public class SpotifyAlbumModel
    {
        public int ID {get; set;}
        public string Name {get; set;}
        public ICollection<SpotifyAlbumModel> Songs{get; set;}
    }
}