using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifyCurrentUserProfile
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
