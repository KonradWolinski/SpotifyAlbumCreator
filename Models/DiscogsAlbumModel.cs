using System.Text.Json.Serialization;

namespace SpotifyAlbumCreator.Models
{
    public class DiscogsAlbumModel
    {
        [JsonPropertyName("id")]
        public int ID {get; set;}
        [JsonPropertyName("title")]
        public string Title {get; set;}
        [JsonPropertyName("year")]
        public string Year {get; set;}
        [JsonPropertyName("thumb")]
        public string? ThumbnailUrl {get; set;}        

        public ICollection<String>? Songs {get; set;}

    }
}