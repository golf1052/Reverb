using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifySavedAlbum
    {
        [JsonProperty("added_at")]
        public DateTimeOffset AddedAt { get; set; }

        [JsonProperty("album")]
        public SpotifyAlbum Album { get; set; }
    }
}
