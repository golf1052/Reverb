using System;
using System.Collections.Generic;
using System.Text;
using Reverb.Models.WebPlayer;

namespace Reverb
{
    public class SpotifyWebPlaybackStateEventArgs : EventArgs
    {
        public SpotifyWebPlaybackState State { get; set; }

        public SpotifyWebPlaybackStateEventArgs()
        {
        }

        public SpotifyWebPlaybackStateEventArgs(SpotifyWebPlaybackState state)
        {
            State = state;
        }
    }
}
