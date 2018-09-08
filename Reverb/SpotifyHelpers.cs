using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flurl;

namespace Reverb
{
    public static class SpotifyHelpers
    {
        public static string SpotifyScopeToString(SpotifyConstants.SpotifyScopes scope)
        {
            if (scope == SpotifyConstants.SpotifyScopes.UserLibraryRead)
            {
                return "user-library-read";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserLibraryModify)
            {
                return "user-library-modify";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.PlaylistReadPrivate)
            {
                return "playlist-read-private";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.PlaylistModifyPublic)
            {
                return "playlist-modify-public";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.PlaylistModifyPrivate)
            {
                return "playlist-modify-private";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.PlaylistReadCollaborative)
            {
                return "playlist-read-collaborative";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserReadRecentlyPlayed)
            {
                return "user-read-recently-played";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserTopRead)
            {
                return "user-top-read";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserReadPrivate)
            {
                return "user-read-private";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserReadEmail)
            {
                return "user-read-email";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserReadBirthdate)
            {
                return "user-read-birthdate";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.Streaming)
            {
                return "streaming";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.AppRemoteControl)
            {
                return "app-remote-control";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserModifyPlaybackState)
            {
                return "user-modify-playback-state";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserReadCurrentlyPlaying)
            {
                return "user-read-currently-playing";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserReadPlaybackState)
            {
                return "user-read-playback-state";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserFollowModify)
            {
                return "user-follow-modify";
            }
            else if (scope == SpotifyConstants.SpotifyScopes.UserFollowRead)
            {
                return "user-follow-read";
            }
            else
            {
                return "unknown-scope";
            }
        }

        public static string GetAuthorizeUrl(string clientId,
            string redirectUri,
            List<SpotifyConstants.SpotifyScopes> scopes = null,
            string state = null)
        {
            string scopesString;
            if (scopes == null)
            {
                scopesString = null;
            }
            else
            {
                scopesString = string.Join(" ", scopes.Select(scope => SpotifyScopeToString(scope)).ToList());
            }
            return new Url(SpotifyConstants.AuthorizeUrl)
                .SetQueryParam("client_id", clientId)
                .SetQueryParam("response_type", "code")
                .SetQueryParam("redirect_uri", redirectUri)
                .SetQueryParam("state", state)
                .SetQueryParam("scope", scopesString);
        }

        public static string GetEncodedAuth(string clientId, string clientSecret)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        }
    }
}
