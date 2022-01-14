namespace SpotifyAlbumCreator.Models
{
    public class AuthorizationCodeResponse
    {
        public string? Code {get; set;}
        public string? State {get; set;}
        public string? Error {get; set;}
    }
}