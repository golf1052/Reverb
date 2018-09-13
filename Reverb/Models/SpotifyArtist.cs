using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifyArtist
    {
        [JsonProperty("external_urls")]
        public Dictionary<string, string> ExternalUrls { get; set; }

        [JsonProperty("followers")]
        public SpotifyFollowers Followers { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<SpotifyImage> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        public SpotifyImage GetLargestImage()
        {
            if (Images == null || Images.Count == 0)
            {
                return null;
            }
            return Images.Aggregate((agg, next) => next.Width * next.Height > agg.Width * agg.Height ? next : agg);
        }

        public SpotifyImage GetSmallestImage()
        {
            if (Images == null || Images.Count == 0)
            {
                return null;
            }
            return Images.Aggregate((agg, next) => next.Width * next.Height < agg.Width * agg.Height ? next : agg);
        }
    }
}
