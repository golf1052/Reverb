using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Reverb.Models
{
    internal class SpotifyDevicesResponse
    {
        [JsonProperty("devices")]
        public List<SpotifyDevice> Devices { get; set; }
    }
}
