namespace SpotifyAlbumCreator.Models
{
    public class AccountModel
    {
        public string? Username {get; set;}
        public bool LoggedIn => !String.IsNullOrEmpty(Username);
        public SpotifyAccessTokenResponse AccessTokenResponse {get; set;}
    }
}