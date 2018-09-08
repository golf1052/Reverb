using Newtonsoft.Json;

namespace Reverb.Models
{
    public class SpotifyCurrentlyPlaying
    {
        [JsonProperty("context")]
        public SpotifyContext Context { get; set; }

        /// <summary>
        /// Progress into the currently playing track, can be null
        /// </summary>
        [JsonProperty("progress_ms")]
        public long? Progress { get; set; }

        [JsonProperty("is_playing")]
        public bool IsPlaying { get; set; }

        [JsonProperty("item")]
        public SpotifyTrack Track { get; set; }

        [JsonProperty("device")]
        public SpotifyDevice Device { get; set; }

        [JsonProperty("shuffle_state")]
        public bool? ShuffleState { get; set; }

        [JsonProperty("repeat_state")]
        public string RepeatState { get; set; }
    }
}
