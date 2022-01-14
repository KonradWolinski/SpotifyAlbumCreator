namespace SpotifyAlbumCreator.Services
{
    public class DiscogsService
    {
        private readonly HttpClient _httpClient;

        public DiscogsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void GetSearchResults(string query)
        {

        }

        public void GetRelease(int id)
        {

        }
    }
}