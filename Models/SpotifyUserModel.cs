using System.Text.Json.Serialization;

namespace SpotifyAlbumCreator.Models
{
    public class SpotifyUserModel
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

    }
}