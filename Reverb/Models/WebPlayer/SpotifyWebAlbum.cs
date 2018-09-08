using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models.WebPlayer
{
    public class SpotifyWebAlbum
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("images")]
        public List<SpotifyWebImage> Images { get; set; }

        public SpotifyWebImage GetLargestImage()
        {
            if (Images == null)
            {
                return null;
            }
            return Images.Aggregate((agg, next) => next.Width * next.Height > agg.Width * agg.Height ? next : agg);
        }

        public SpotifyWebImage GetSmallestImage()
        {
            if (Images == null)
            {
                return null;
            }
            return Images.Aggregate((agg, next) => next.Width * next.Height < agg.Width * agg.Height ? next : agg);
        }
    }
}
