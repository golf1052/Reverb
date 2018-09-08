using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models.WebPlayer
{
    public class SpotifyWebDisallows
    {
        [JsonProperty("pausing")]
        public bool? Pausing { get; set; }

        [JsonProperty("peeking_next")]
        public bool? PeekingNext { get; set; }

        [JsonProperty("peeking_prev")]
        public bool? PeekingPrev { get; set; }

        [JsonProperty("resuming")]
        public bool? Resuming { get; set; }

        [JsonProperty("seeking")]
        public bool? Seeking { get; set; }

        [JsonProperty("skipping_next")]
        public bool? SkippingNext { get; set; }

        [JsonProperty("skipping_prev")]
        public bool? SkippingPrev { get; set; }
    }
}
