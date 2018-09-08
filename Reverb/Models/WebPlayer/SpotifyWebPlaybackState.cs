using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models.WebPlayer
{
    public class SpotifyWebPlaybackState
    {
        [JsonProperty("context")]
        public SpotifyWebContext Context { get; set; }

        [JsonProperty("disallows")]
        public SpotifyWebDisallows Disallows { get; set; }

        [JsonProperty("paused")]
        public bool Paused { get; set; }

        [JsonProperty("bitrate")]
        public int Bitrate { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("repeat_mode")]
        public int RepeatMode { get; set; }

        [JsonProperty("shuffle")]
        public bool Shuffle { get; set; }

        [JsonProperty("track_window")]
        public SpotifyWebTrackWindow TrackWindow { get; set; }
    }
}
