using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models
{
    internal class SpotifyArtistsResponse
    {
        [JsonProperty("artists")]
        public List<SpotifyArtist> Artists { get; set; }
    }
}
