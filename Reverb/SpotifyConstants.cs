using System;
using System.Collections.Generic;
using System.Text;
using Flurl;

namespace Reverb
{
    public class SpotifyConstants
    {
        public const string AuthorizeUrl = "https://accounts.spotify.com/authorize";
        public const string RequestAccessTokenUrl = "https://accounts.spotify.com/api/token";
        public const string BaseV1ApiUrl = "https://api.spotify.com/v1";

        public enum SpotifyScopes
        {
            UserLibraryRead,
            UserLibraryModify,
            PlaylistReadPrivate,
            PlaylistModifyPublic,
            PlaylistModifyPrivate,
            PlaylistReadCollaborative,
            UserReadRecentlyPlayed,
            UserTopRead,
            UserReadPrivate,
            UserReadEmail,
            UserReadBirthdate,
            Streaming,
            AppRemoteControl,
            UserModifyPlaybackState,
            UserReadCurrentlyPlaying,
            UserReadPlaybackState,
            UserFollowModify,
            UserFollowRead
        }
    }
}
