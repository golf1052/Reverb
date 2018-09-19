using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models
{
    internal class SpotifyNewReleasesResponse
    {
        [JsonProperty("albums")]
        public SpotifyPagingObject<SpotifyAlbum> Albums { get; set; }
    }
}
