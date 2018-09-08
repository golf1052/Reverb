using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifyImage
    {
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
