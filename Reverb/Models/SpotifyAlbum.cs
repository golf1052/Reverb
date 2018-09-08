using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifyAlbum
    {
        [JsonProperty("album_type")]
        public string AlbumType { get; set; }

        [JsonProperty("artists")]
        public List<SpotifyArtist> Artists { get; set; }

        [JsonProperty("available_markets")]
        public List<string> AvailableMarkets { get; set; }

        [JsonProperty("copyrights")]
        public List<SpotifyCopyright> Copyrights { get; set; }

        [JsonProperty("external_ids")]
        public Dictionary<string, string> ExternalIds { get; set; }

        [JsonProperty("external_urls")]
        public Dictionary<string, string> ExternalUrls { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<SpotifyImage> Images { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("release_date_precision")]
        public string ReleaseDatePrecision { get; set; }

        [JsonProperty("restrictions")]
        public Dictionary<string, string> Restrictions { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        public SpotifyImage GetLargestImage()
        {
            if (Images == null)
            {
                return null;
            }
            return Images.Aggregate((agg, next) => next.Width * next.Height > agg.Width * agg.Height ? next : agg);
        }

        public SpotifyImage GetSmallestImage()
        {
            if (Images == null)
            {
                return null;
            }
            return Images.Aggregate((agg, next) => next.Width * next.Height < agg.Width * agg.Height ? next : agg);
        }
    }
}
