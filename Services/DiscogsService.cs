using SpotifyAlbumCreator.Models;

namespace SpotifyAlbumCreator.Services
{
    public class DiscogsService
    {
        private readonly HttpClient _httpClient;

        public DiscogsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DiscogsAlbumModel>> GetSearchResults(string key, string secret, string query)
        {
            HttpRequestMessage request = new(HttpMethod.Get, 
                $"{Models.UriValues.Discogs}/database/search?q={query}&type=release&per_page=10&key={key}&secret={secret}");
            request.Headers.TryAddWithoutValidation("Content-Type", "text/javascript");
            request.Headers.TryAddWithoutValidation("User-Agent", "SpotifyPlaylistCreator/1.0");


            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            
            //parse it to get album models
            
            return new();
        }

        public void GetRelease(int id)
        {

        }
    }
}