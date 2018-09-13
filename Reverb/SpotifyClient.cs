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

        public async Task<SpotifyAlbum> GetAlbum(string id, string market = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("albums", id)
                .SetQueryParam("market", market);

            return await MakeAuthorizedSpotifyRequest<SpotifyAlbum>(url, HttpMethod.Get);
        }

        public async Task<SpotifyPagingObject<SpotifySavedAlbum>> GetUserSavedAlbums(int limit = 20, int offset = 0, string market = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "albums");

            return await MakeAuthorizedSpotifyRequest<SpotifyPagingObject<SpotifySavedAlbum>>(url, HttpMethod.Get);
        }

        public async Task<SpotifyArtist> GetArtist(string id)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("artists", id);

            return await MakeAuthorizedSpotifyRequest<SpotifyArtist>(url, HttpMethod.Get);
        }

        public async Task<SpotifyPagingObject<SpotifyAlbum>> GetArtistsAlbums(string id,
            List<SpotifyConstants.SpotifyArtistIncludeGroups> includeGroups = null,
            string market = null,
            int? limit = null,
            int? offset = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("artists", id, "albums");
            if (includeGroups != null)
            {
                url.SetQueryParam("include_groups", string
                    .Join(",", includeGroups.Select(includeGroup => SpotifyHelpers.SpotifyArtistIncludeGroupsToString(includeGroup))));
            }
            url.SetQueryParam("market", market)
                .SetQueryParam("limit", limit)
                .SetQueryParam("offset", offset);

            return await MakeAuthorizedSpotifyRequest<SpotifyPagingObject<SpotifyAlbum>>(url, HttpMethod.Get);
        }

        public async Task<List<SpotifyTrack>> GetArtistsTopTracks(string id, string market)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("artists", id, "top-tracks")
                .SetQueryParam("market", market);

            return (await MakeAuthorizedSpotifyRequest<SpotifyArtistTopTracksResponse>(url, HttpMethod.Get)).Tracks;
        }

        public async Task<List<SpotifyArtist>> GetArtistsRelatedArtists(string id)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("artists", id, "related-artists");

            return (await MakeAuthorizedSpotifyRequest<SpotifyArtistsResponse>(url, HttpMethod.Get)).Artists;
        }

        public async Task<List<SpotifyArtist>> GetArtists(List<string> ids)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegment("artists")
                .SetQueryParams("ids", string.Join(",", ids));

            return (await MakeAuthorizedSpotifyRequest<SpotifyArtistsResponse>(url, HttpMethod.Get)).Artists;
        }

        public async Task<List<SpotifyDevice>> GetUserDevices()
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player", "devices");

            return (await MakeAuthorizedSpotifyRequest<SpotifyDevicesResponse>(url, HttpMethod.Get)).Devices;
        }

        public async Task SetVolume(int volumePercent, string deviceId = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player", "volume")
                .SetQueryParam("volume_percent", volumePercent)
                .SetQueryParam("device_id", deviceId);

            await MakeAuthorizedSpotifyPut(url, null);
        }

        public async Task Play(string deviceId = null)
        {
            await Play(deviceId, null, null, string.Empty, null);
        }

        public async Task Play(string deviceId = null,
            string contextUri = null)
        {
            await Play(deviceId, contextUri, null, string.Empty, null);
        }

        public async Task Play(string deviceId = null,
            string contextUri = null,
            List<string> uris = null,
            int? offset = null,
            int? position = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player", "play")
                .SetQueryParam("device_id", deviceId);
            JObject bodyParams = SetPlayBodyParams(contextUri, uris, position);
            SetOffset(offset, bodyParams);
            await Play(url, bodyParams);
        }

        public async Task Play(string deviceId = null,
            string contextUri = null,
            List<string> uris = null,
            string offset = null,
            int? position = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegments("me", "player", "play")
                .SetQueryParam("device_id", deviceId);
            JObject bodyParams = SetPlayBodyParams(contextUri, uris, position);
            SetOffset(offset, bodyParams);
            await Play(url, bodyParams);
        }

        private JObject SetPlayBodyParams(string contextUri = null,
            List<string> uris = null,
            int? position = null)
        {
            JObject bodyParams = new JObject();
            if (contextUri != null)
            {
                bodyParams["context_uri"] = contextUri;
            }
            if (uris != null)
            {
                bodyParams["uris"] = new JArray(uris);
            }
            if (position.HasValue)
            {
                bodyParams["position_ms"] = position.Value;
            }
            return bodyParams;
        }

        private void SetOffset(int? offset, JObject bodyParams)
        {
            if (offset.HasValue)
            {
                JObject position = new JObject();
                position["position"] = offset;
                bodyParams["offset"] = position;
            }
        }

        private void SetOffset(string offset, JObject bodyParams)
        {
            if (!string.IsNullOrEmpty(offset))
            {
                JObject uri = new JObject();
                uri["uri"] = offset;
                bodyParams["offset"] = uri;
            }
        }

        private async Task Play(Url url, JObject bodyParams)
        {
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

        public async Task<SpotifySearch> Search(string query,
            List<SpotifyConstants.SpotifySearchTypes> type,
            string market = null,
            int? limit = null,
            int? offset = null)
        {
            Url url = new Url(SpotifyConstants.BaseV1ApiUrl)
                .AppendPathSegment("search")
                .SetQueryParam("q", query)
                .SetQueryParam("type", string.Join(",", type.Select(t => SpotifyHelpers.SpotifySearchTypeToString(t))))
                .SetQueryParam("market", market)
                .SetQueryParam("limit", limit)
                .SetQueryParam("offset", offset);

            return await MakeAuthorizedSpotifyRequest<SpotifySearch>(url, HttpMethod.Get);
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
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {

            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {

            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                SpotifyErrorObject spotifyError = JsonConvert.DeserializeObject<SpotifyErrorObject>(await responseMessage.Content.ReadAsStringAsync());
                throw new SpotifyException(spotifyError.Error);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
