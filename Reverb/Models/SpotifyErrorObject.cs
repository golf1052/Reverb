using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models
{
    internal class SpotifyErrorObject
    {
        [JsonProperty("error")]
        public SpotifyError Error { get; set; }
    }
}
