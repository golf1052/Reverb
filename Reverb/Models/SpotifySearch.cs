using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifySearch
    {
        [JsonProperty("albums")]
        public SpotifyPagingObject<SpotifyAlbum> Albums { get; set; }

        [JsonProperty("artists")]
        public SpotifyPagingObject<SpotifyArtist> Artists { get; set; }

        [JsonProperty("tracks")]
        public SpotifyPagingObject<SpotifyTrack> Tracks { get; set; }
    }
}
