using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifyCopyright
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
