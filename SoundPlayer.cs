using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DiaporamaPlayer
{
    internal class SoundPlayer
    {
        private readonly MediaPlayer mediaPlayer = new MediaPlayer();
        private readonly SemaphoreSlim manualResetEventSlim = new SemaphoreSlim(0);

        public SoundPlayer()
        {
            mediaPlayer.MediaEnded += OnMediaEnded;
        }

        private void OnMediaEnded(object? sender, EventArgs e)
        {
            manualResetEventSlim.Release();
        }

        public void PlayIfSet(string songPath)
        {
            if (!string.IsNullOrEmpty(songPath))
            {
                mediaPlayer.Open(new Uri(songPath));
                mediaPlayer.Play();
            }
        }

        public async Task WaitUntilFinishedAsync()
        {
            await manualResetEventSlim.WaitAsync();
        }
    }
}
