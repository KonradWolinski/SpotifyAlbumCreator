using System.Net.Http.Headers;
using SpotifyAlbumCreator.Models;

namespace SpotifyAlbumCreator.Services
{
    public class SpotifyService
    {
        private readonly HttpClient _httpClient;

        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public void AddTrack(string playlistId, string trackId)
        {
            throw new NotImplementedException();
        }
        public string MakePlaylist()
        {
            //returns playlist ID
            throw new NotImplementedException();
        }
        public string FindAlbum(string artist, string title)
        {
            //return ID
            throw new NotImplementedException();
        }
        public async Task<string> RequestAccessToken(string code, string redirectUri, string clientId, string clientSecret)
        {
            string grantType = "authorization_code";
            var bodyValues = new Dictionary<string, string>() { { "grant_type", grantType }, { "code", code }, 
            { "redirect_uri", redirectUri }, { "client_id", clientId }, { "client_secret", clientSecret }};

            var content = new FormUrlEncodedContent(bodyValues);
            content.Headers.ContentType = new("application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync(UriValues.SpotifyApiToken, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;

        }
        public async Task<string?> GetUser(string accessToken)
        {
            HttpRequestMessage request = new(HttpMethod.Get, $"{UriValues.Spotify}/me");
            request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

    }
}