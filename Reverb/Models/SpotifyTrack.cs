using System.Collections.Generic;
using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifyTrack
    {
        [JsonProperty("album")]
        public SpotifyAlbum Album { get; set; }

        [JsonProperty("artists")]
        public List<SpotifyArtist> Artists { get; set; }

        [JsonProperty("available_markets")]
        public List<string> AvailableMarkets { get; set; }

        [JsonProperty("disk_number")]
        public int DiskNumber { get; set; }

        /// <summary>
        /// Duration of the track in milliseconds
        /// </summary>
        [JsonProperty("duration_ms")]
        public long Duration { get; set; }

        [JsonProperty("explicit")]
        public bool Explicit { get; set; }

        [JsonProperty("external_ids")]
        public Dictionary<string, string> ExternalIds { get; set; }

        [JsonProperty("external_urls")]
        public Dictionary<string, string> ExternalUrls { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_playable")]
        public bool IsPlayable { get; set; }

        [JsonProperty("linked_from")]
        public SpotifyTrackLink LinkedFrom { get; set; }

        [JsonProperty("restrictions")]
        public Dictionary<string, string> Restrictions { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }

        [JsonProperty("track_number")]
        public int TrackNumber { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("is_local")]
        public bool IsLocal { get; set; }
    }
}
