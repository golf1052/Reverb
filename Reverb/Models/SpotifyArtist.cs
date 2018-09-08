using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifyArtist
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
