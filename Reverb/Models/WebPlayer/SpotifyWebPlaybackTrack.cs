using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models.WebPlayer
{
    public class SpotifyWebPlaybackTrack
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("duration_ms")]
        public int Duration { get; set; }

        [JsonProperty("is_playable")]
        public string IsPlayable { get; set; }

        [JsonProperty("album")]
        public SpotifyWebAlbum Album { get; set; }

        [JsonProperty("artists")]
        public List<SpotifyWebArtist> Artists { get; set; }
    }
}
