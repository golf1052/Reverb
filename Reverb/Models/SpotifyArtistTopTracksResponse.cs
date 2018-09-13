using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models
{
    internal class SpotifyArtistTopTracksResponse
    {
        [JsonProperty("tracks")]
        public List<SpotifyTrack> Tracks { get; set; }
    }
}
