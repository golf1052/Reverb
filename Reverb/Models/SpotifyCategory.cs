using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifyCategory
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("icons")]
        public List<SpotifyImage> Icons { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public SpotifyImage GetLargestIcon()
        {
            if (Icons == null || Icons.Count == 0)
            {
                return null;
            }
            return Icons.Aggregate((agg, next) => next.Width * next.Height > agg.Width * agg.Height ? next : agg);
        }

        public SpotifyImage GetSmallestIcon()
        {
            if (Icons == null || Icons.Count == 0)
            {
                return null;
            }
            return Icons.Aggregate((agg, next) => next.Width * next.Height < agg.Width * agg.Height ? next : agg);
        }
    }
}
