using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models.WebPlayer
{
    public class SpotifyWebArtist
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
