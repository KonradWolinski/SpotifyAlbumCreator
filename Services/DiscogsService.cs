using System.Text.Json;
using SpotifyAlbumCreator.Models;

namespace SpotifyAlbumCreator.Services
{
    public class DiscogsService
    {

        public class DiscogJsonResults
        {
            public List<DiscogJsonAlbumResponse> results { get; set; }
        }
        public class DiscogJsonAlbumResponse
        {
            public int id { get; set; }
            public string year { get; set; }
            public string title { get; set; }
            public string resource_url {get; set;}
            public string thumb {get; set;}
        }

        public class DiscogsJsonReleaseResponse
        {
            public List<DiscogsJsonTracklist> tracklist { get; set; }
        }

        public class DiscogsJsonTracklist
        {
            public string title { get; set; }
            public string duration { get; set; }
        }


        private readonly HttpClient _httpClient;

        public DiscogsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DiscogsAlbumModel>> GetSearchResults(string key, string secret, string query)
        {
            HttpRequestMessage request = new(HttpMethod.Get,
                $"{Models.UriValues.Discogs}/database/search?q={query}&format=album&type=release&per_page=10&key={key}&secret={secret}");
            AddHeaders(request);


            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var deserializedAlbumResponse = JsonSerializer.Deserialize<DiscogJsonResults>(responseString, options)
                                        ?.results;

            if (deserializedAlbumResponse == null)
                return new();

            List<DiscogsAlbumModel> responseModel = new();

            for (int i = 0; i < deserializedAlbumResponse.Count; i++)
            {
                var album = deserializedAlbumResponse[i];
                if (album == null)
                    continue;
                HttpRequestMessage releaseRequest = new(HttpMethod.Get, album.resource_url);
                AddHeaders(releaseRequest);
                response = await _httpClient.SendAsync(releaseRequest);
                responseString = await response.Content.ReadAsStringAsync();

                var deserializedTrackResponse = JsonSerializer.Deserialize<DiscogsJsonReleaseResponse>(responseString)
                                                ?.tracklist;
                if (deserializedTrackResponse == null)
                    continue;

                List<string> tracklist = new();
                foreach (var track in deserializedTrackResponse)
                {
                    tracklist.Add($"{track.title} ({track.duration})");
                }
                responseModel.Add(new DiscogsAlbumModel {Year = album.year, Title = album.title, 
                                                 ThumbnailUrl = album.thumb, Songs = tracklist});
            }

            return responseModel;
        }
        private void AddHeaders(HttpRequestMessage request)
        {
            request.Headers.TryAddWithoutValidation("Content-Type", "text/javascript");
            request.Headers.TryAddWithoutValidation("User-Agent", "SpotifyPlaylistCreator/1.0");
        }
    }

}
