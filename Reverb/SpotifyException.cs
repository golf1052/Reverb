using System;
using System.Collections.Generic;
using System.Text;
using Reverb.Models;

namespace Reverb
{
    public class SpotifyException : Exception
    {
        public SpotifyError Error { get; set; }

        public SpotifyException(SpotifyError error) : base(error.Message)
        {
            Error = error;
        }
    }
}
