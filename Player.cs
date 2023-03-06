using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DiaporamaPlayer
{
    internal class Player
    {
        private readonly Panel destinationPanel;
        private readonly string scriptFullpath;
        private readonly PolaroidAnimator polaroidAnimator;
        private readonly PolaroidFactory polaroidFactory;
        private readonly SoundPlayer soundPlayer;

        public Player(Panel destinationPanel, string scriptFullpath)
        {
            this.destinationPanel = destinationPanel;
            this.scriptFullpath = scriptFullpath;
            this.polaroidAnimator = new PolaroidAnimator(destinationPanel.RenderSize); // TODO: not instantiate here
            this.polaroidFactory = new PolaroidFactory(destinationPanel.RenderSize);  // TODO: not instantiate here
            this.soundPlayer = new SoundPlayer();
        }

        public async void Play()
        {
            DiaporamaScript script = ReadScriptFromFile(scriptFullpath);
            var fadeoutAnimator = new FadeoutAnimator(this.destinationPanel, script.FadeoutDuration);
            var polaroidRegistrer = new LayoutRegistrer<PolaroidUserControl>(script.MaximumPicturesPerLayout);

#if !DEBUG
            this.soundPlayer.PlayIfSet(script.SongFullpath);
#endif

            await Task.Delay(script.StartTemporisationDuration);

            foreach (var currentStep in script.Steps)
            {
                var polaroid = await PlayStepAsync(script, currentStep);

                FadeoutOutOldElementIfRequired(polaroidRegistrer, currentStep.FinalLayout, polaroid, fadeoutAnimator);
            }
        }

        private static void FadeoutOutOldElementIfRequired(
            LayoutRegistrer<PolaroidUserControl> polaroidRegistrer,
            FinalLayout finalLayout, 
            PolaroidUserControl newPolaroid, 
            FadeoutAnimator fadeoutAnimator)
        {
            var polaroidToRemove = polaroidRegistrer.RegisterNewAndGetOldestElement(newPolaroid, finalLayout);
            if (polaroidToRemove != null)
            {
                fadeoutAnimator.FadeOutAndRemove(polaroidToRemove);
            }
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
