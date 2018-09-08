using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models.WebPlayer
{
    public class SpotifyWebContext
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }
    }
}
