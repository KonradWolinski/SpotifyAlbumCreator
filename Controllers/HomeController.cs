using Microsoft.AspNetCore.Mvc;
using SpotifyAlbumCreator.Models;
using SpotifyAlbumCreator.Services;
using Microsoft.AspNetCore.Http.Extensions;
using System.Web;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace SpotifyAlbumCreator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly SpotifyService _spotifyService;
        private readonly DiscogsService _discogsService;

        private static string? state { get; set; }

        private string loginUrl = "https://accounts.spotify.com/api/token";
        private List<string> spotifyScopes = new()
        {
            "playlist-modify-private",
            "playlist-read-private",
            "user-read-private"
        };

        public HomeController(IConfiguration configuration, SpotifyService spotifyService, DiscogsService discogsService)
        {
            _configuration = configuration;
            _spotifyService = spotifyService;
            _discogsService = discogsService;
        }


        // public IActionResult Index() => View();
        public IActionResult Index()
        {
            string? accountModelSerialized = TempData["accountModel"] as string;
            AccountModel? accountModel = null;
            if (accountModelSerialized != null)
            {
                accountModel = JsonSerializer.Deserialize<AccountModel>(accountModelSerialized);
            }
            return View(accountModel);
        }
        public IActionResult Login()
        {
            state = GetState();
            Dictionary<string, string> request = new()
            {
                { "client_id", _configuration["ClientID:Spotify"] },
                { "response_type", "code" },
                { "scope", String.Join("%20", spotifyScopes) },
                { "state", state },
                { "redirect_uri", Request.GetEncodedUrl().Replace("Login", "Callback") }
            };
            //change redirectUri to callback, in that get function get token, return to Index

            string url = UriValues.SpotifyAuthorization + string.Join('&', request.Select(val => val.Key + "=" + val.Value).ToArray());
            return Redirect(url);
        }

        public async Task<IActionResult> Callback(AuthorizationCodeResponse res)
        {
            if (res.State != state || !String.IsNullOrEmpty(res.Error) || res.Code == null)
                return RedirectToAction("Index");

            var callbackIndex = Request.GetEncodedUrl().IndexOf("Callback");
            var redirectUri = Request.GetEncodedUrl().Substring(0, callbackIndex + "Callback".Length);

            var token = await _spotifyService.RequestAccessToken(res.Code, redirectUri,
                _configuration["ClientID:Spotify"], _configuration["Secrets:SpotifyApi"]);
            //replace with url in appsettings

            if (token == null)
                return RedirectToAction("Index");
            //null check

            SpotifyAccessTokenResponse accessTokenResponse =
                JsonSerializer.Deserialize<SpotifyAccessTokenResponse>(token);

            var userJson = await _spotifyService.GetUser(accessTokenResponse.access_token);

            SpotifyUserModel? userModel = JsonSerializer.Deserialize<SpotifyUserModel>(userJson);

            AccountModel accountModel = new() { Username = userModel.DisplayName, AccessTokenResponse = accessTokenResponse };

            //get token
            TempData["accountModel"] = JsonSerializer.Serialize<AccountModel>(accountModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Index(string searchQuery)
        {
            var discogsResponse = await SearchDiscogs(searchQuery);
            TempData["discogsModel"] = JsonSerializer.Serialize<List<DiscogsAlbumModel>>(discogsResponse);
            return RedirectToAction("Search");
            // return RedirectToAction("Search", new { discogsModel = discogsResponse });

            // var spotifyId = FindSpotifyAlbumId(discogsResponseConverted); //get arist and title from discogsResponse
            // var spotifyModel = SearchSpotify(spotifyId);
            // var created = CreatePlaylist(spotifyModel); //possibly convert track IDs to array and pass that
            //create playlist
            //
        }
        public IActionResult Search()
        {
            string? discogsModelSerialized = TempData["discogsModel"] as string;
            List<DiscogsAlbumModel>? discogsModel = null;
            if(discogsModelSerialized != null)
            {
                discogsModel = JsonSerializer.Deserialize<List<DiscogsAlbumModel>>(discogsModelSerialized);
            }
            return View(discogsModel);
        }
        public IActionResult Tracklist(ICollection<string> tracklist)
        {
            //find song on spotify
            //button on the bottom of view lets you add the playlist
            //after that return to Index

            return View(tracklist);
        }

        private async Task<List<DiscogsAlbumModel>> SearchDiscogs(string querry)
        {
            var searchResults = await _discogsService.GetSearchResults(_configuration["ClientID:Discogs"], _configuration["Secrets:DiscogsApi"],
            querry);

            return searchResults;
        }

        private string FindSpotifyAlbumId(string query)
        {
            //returns id
            return null;
        }

        private SpotifyAlbumModel SearchSpotify(string id)
        {
            //returns album tracklist ids
            return new SpotifyAlbumModel();
        }
        private bool CreatePlaylist(SpotifyAlbumModel tracklist)
        {
            //returns true if created
            return false;
        }
        private string GetState()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }


    }
}