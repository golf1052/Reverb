using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifyDevice
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("is_private_session")]
        public bool IsPrivateSession { get; set; }

        [JsonProperty("is_restricted")]
        public bool IsRestricted { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("volume_percent")]
        public int? VolumePercent { get; set; }
    }
}
