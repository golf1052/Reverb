using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models.WebPlayer
{
    public class SpotifyWebTrackWindow
    {
        [JsonProperty("current_track")]
        public SpotifyWebPlaybackTrack CurrentTrack { get; set; }

        [JsonProperty("previous_tracks")]
        public List<SpotifyWebPlaybackTrack> PreviousTracks { get; set; }

        [JsonProperty("next_tracks")]
        public List<SpotifyWebPlaybackTrack> NextTracks { get; set; }
    }
}
