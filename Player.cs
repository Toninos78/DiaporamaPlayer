using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DiaporamaPlayer
{
    internal class Player
    {
        private readonly Panel destinationPanel;
        private readonly string scriptFullpath;
        private readonly MediaPlayer mediaPlayer = new MediaPlayer();
        private readonly PolaroidAnimator polaroidAnimator;
        private readonly PolaroidFactory polaroidFactory;

        public Player(Panel destinationPanel, string scriptFullpath)
        {
            this.destinationPanel = destinationPanel;
            this.scriptFullpath = scriptFullpath;
            this.polaroidAnimator = new PolaroidAnimator(destinationPanel.RenderSize); // TODO: not instantiate here
            this.polaroidFactory = new PolaroidFactory(destinationPanel.RenderSize);  // TODO: not instantiate here
        }

        public async void Play()
        {
            DiaporamaScript script = ReadScriptFromFile(scriptFullpath);

#if !DEBUG
            PlaySongIfDefined(script.SongFullpath);
#endif

            await ExecuteStartTemporisationDelay(script);

            var polaroidDictionary = new Dictionary<string, List<PolaroidUserControl>>();

            foreach (var currentStep in script.Steps)
            {
                var polaroid = await PlayStepAsync(script, currentStep);

                var toRemove = RegisterNewPolaroidAndGetPolaroidToRemove(polaroidDictionary, polaroid, currentStep.FinalLayout);
                if (toRemove != null)
                {
                    this.destinationPanel.Children.Remove(toRemove);
                }
            }
        }

        // TODO: extract in a dedicated service
        private PolaroidUserControl? RegisterNewPolaroidAndGetPolaroidToRemove(Dictionary<string, List<PolaroidUserControl>> polaroidDictionary, PolaroidUserControl newPolaroid, FinalLayout layout)
        {
            var orientation = newPolaroid.ActualHeight > newPolaroid.ActualWidth ? Orientation.Portrait : Orientation.Landscape;
            var key = $"{orientation}-{layout}";

            if (!polaroidDictionary.ContainsKey(key))
            {
                polaroidDictionary.Add(key, new List<PolaroidUserControl>());
            }

            polaroidDictionary[key].Add(newPolaroid);

            if (polaroidDictionary[key].Count > 2)
            {
                var polaroidToRemove = polaroidDictionary[key].ElementAt(0);
                polaroidDictionary[key].Remove(polaroidToRemove);

                return polaroidToRemove;
            }

            return null;
        }

        private static async Task ExecuteStartTemporisationDelay(DiaporamaScript script)
        {
            await Task.Delay(script.StartTemporisationDuration);
        }

        private async Task<PolaroidUserControl> PlayStepAsync(DiaporamaScript script, DiaporamaStep currentStep)
        {
            float pictureMarginRatio = currentStep.FinalLayout == FinalLayout.Full ? script.BigPictureMarginRatio : script.SmallPictureMarginRatio;
            string pictureFullPath = Path.Combine(script.ImageFolder, currentStep.Filename);

            PolaroidUserControl polaroid = CreatePolaroid(polaroidFactory, pictureFullPath, pictureMarginRatio);

            polaroidAnimator.Animate(currentStep, polaroid, script.MaximumAbsoluteStartAngleDeviation, script.MaximumAbsoluteEndAngleDeviation);

            await Task.Delay(currentStep.Duration);

            return polaroid;
        }

        // TODO: extract in dedicated service
        private void PlaySongIfDefined(string songPath)
        {
            if (!string.IsNullOrEmpty(songPath))
            {
                mediaPlayer.Open(new Uri(songPath));
                mediaPlayer.Play();
            }
        }

        private static DiaporamaScript ReadScriptFromFile(string source)
        {
            var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
            var rawText = File.ReadAllText(source);

            return JsonSerializer.Deserialize<DiaporamaScript>(rawText, options)!;
        }

        private PolaroidUserControl CreatePolaroid(PolaroidFactory factory, string pictureFullpath, float pictureMarginRatio)
        {
            PolaroidUserControl polaroid = factory.CreatePolaroid(pictureFullpath, pictureMarginRatio);
            destinationPanel.Children.Add(polaroid);
            destinationPanel.UpdateLayout();

            return polaroid;
        }
    }
}
