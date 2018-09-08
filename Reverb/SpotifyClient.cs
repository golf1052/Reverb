using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json;
using System.Linq;
using Reverb.Models;
using System.Net.Http.Headers;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Reverb
{
    public class SpotifyClient
    {
        private HttpClient httpClient;
        private string clientId;
        private string clientSecret;
        private string redirectUrl;
        private string refreshToken;
        
        public string AccessToken { get; private set; }
        public DateTimeOffset AccessTokenExpiresAt { get; private set; }

        public SpotifyClient(string clientId, string clientSecret, string redirectUrl)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUrl = redirectUrl;
            httpClient = new HttpClient();
        }

        public string GetAuthorizeUrl(List<SpotifyConstants.SpotifyScopes> scopes = null, string state = null)
        {
            return SpotifyHelpers.GetAuthorizeUrl(clientId, redirectUrl, scopes, state);
        }

        public async Task ProcessRedirect(Uri uri, string state)
        {
            Url url = new Url(uri.ToString());
            if (url.QueryParams.ContainsKey("error"))
            {
                // TODO: process error
                throw new Exception();
            }
            string code = (string)url.QueryParams["code"];
            string urlState = null;
            if (url.QueryParams.ContainsKey("state"))
            {
                urlState = (string)url.QueryParams["state"];
            }

            if (urlState != state)
            {
                // TODO: Error handling
                throw new Exception();
            }

            await RequestAccessToken(code);
        }

        private async Task RequestAccessToken(string code)
        {
            FormUrlEncodedContent tokenRequestContent = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", redirectUrl }
            });
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                SpotifyHelpers.GetEncodedAuth(clientId, clientSecret));
            HttpResponseMessage responseMessage = await httpClient.PostAsync(SpotifyConstants.RequestAccessTokenUrl,
                tokenRequestContent);
            await ProcessAuthorizationResponse(responseMessage);
        }

        private async Task ProcessAuthorizationResponse(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                // TODO: error handling
                throw new Exception();
            }
            SpotifyAuthorizationResponse response = JsonConvert.DeserializeObject<SpotifyAuthorizationResponse>(
                await responseMessage.Content.ReadAsStringAsync());
            AccessTokenExpiresAt = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn);
            if (response.RefreshToken != null)
            {
                refreshToken = response.RefreshToken;
            }
            AccessToken = response.AccessToken;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(response.TokenType, AccessToken);
        }

        public async Task<SpotifyPagingObject<SpotifySavedAlbum>> GetUserSavedAlbums(int limit = 20, int offset = 0, string market = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "albums");

            return await MakeAuthorizedSpotifyRequest<SpotifyPagingObject<SpotifySavedAlbum>>(url, HttpMethod.Get);
        }

        public async Task<List<SpotifyDevice>> GetUserDevices()
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player", "devices");

            return (await MakeAuthorizedSpotifyRequest<SpotifyDevicesResponse>(url, HttpMethod.Get)).Devices;
        }

        public async Task Play(string deviceId = null,
            string contextUri = null,
            List<string> uris = null,
            JObject offset = null,
            int? position = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player", "play")
                .SetQueryParam("device_id", deviceId);
            JObject bodyParams = new JObject();
            if (contextUri != null)
            {
                bodyParams["context_uri"] = contextUri;
            }
            await MakeAuthorizedSpotifyPut(url, new StringContent(bodyParams.ToString()));
        }

        public async Task Pause(string deviceId = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player", "pause")
                .SetQueryParam("device_id", deviceId);

            await MakeAuthorizedSpotifyPut(url, null);
        }

        public async Task Next(string deviceId = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player", "next")
                .SetQueryParam("device_id", deviceId);

            await MakeAuthorizedSpotifyPost(url, null);
        }

        public async Task Previous(string deviceId = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player", "previous")
                .SetQueryParam("device_id", deviceId);

            await MakeAuthorizedSpotifyPost(url, null);
        }

        public async Task<SpotifyCurrentlyPlaying> GetCurrentlyPlayingPlayer(string market = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player")
                .SetQueryParam("market", market);

            return await MakeAuthorizedSpotifyRequest<SpotifyCurrentlyPlaying>(url, HttpMethod.Get);
        }

        public async Task<SpotifyCurrentlyPlaying> GetCurrentlyPlaying(string market = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player", "currently-playing")
                .SetQueryParam("market", market);

            return await MakeAuthorizedSpotifyRequest<SpotifyCurrentlyPlaying>(url, HttpMethod.Get);
        }

        public async Task TransferPlayback(string deviceId, bool? play = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player");
            JObject bodyParams = new JObject();
            bodyParams["device_ids"] = new JArray(deviceId);
            if (play.HasValue)
            {
                bodyParams["play"] = play.Value;
            }

            await MakeAuthorizedSpotifyPut(url, new StringContent(bodyParams.ToString()));
        }

        public async Task<SpotifyPagingObject<T>> GetNextPage<T>(SpotifyPagingObject<T> pagingObject)
        {
            return await MakeAuthorizedSpotifyRequest<SpotifyPagingObject<T>>(pagingObject.Next, HttpMethod.Get);
        }

        private async Task<T> MakeAuthorizedSpotifyRequest<T>(string url, HttpMethod method)
        {
            if (method.Method == "GET")
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return default(T);
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(T);
                }
                else
                {
                    throw new Exception();
                }
            }
            else if (method.Method == "PUT")
            {
                HttpResponseMessage responseMessage = await httpClient.PutAsync(url, null);
                return default(T);
            }
            else if (method.Method == "POST")
            {
                HttpResponseMessage responseMessage = await httpClient.PostAsync(url, null);
                return default(T);
            }
            else if (method.Method == "DELETE")
            {
                HttpResponseMessage responseMessage = await httpClient.DeleteAsync(url);
                return default(T);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private async Task MakeAuthorizedSpotifyPost(string url, HttpContent content)
        {
            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, content);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {

            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {

            }
            else
            {
                throw new Exception();
            }
        }

        private async Task MakeAuthorizedSpotifyPut(string url, HttpContent content)
        {
            HttpResponseMessage responseMessage = await httpClient.PutAsync(url, content);
            Debug.WriteLine(await responseMessage.Content.ReadAsStringAsync());
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {

            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {

            }
            else
            {
                throw new Exception();
            }
        }
    }
}
