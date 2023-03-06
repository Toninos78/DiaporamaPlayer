using System;
using System.Windows.Media;

namespace DiaporamaPlayer
{
    internal class SoundPlayer
    {
        private readonly MediaPlayer mediaPlayer = new MediaPlayer();

        public void PlayIfSet(string songPath)
        {
            if (!string.IsNullOrEmpty(songPath))
            {
                mediaPlayer.Open(new Uri(songPath));
                mediaPlayer.Play();
            }
        }
    }
}
