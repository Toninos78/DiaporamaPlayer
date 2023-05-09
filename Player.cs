using System;
using System.IO;
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
        private readonly ScriptReader scriptReader;

        public Player(Panel destinationPanel, string scriptFullpath)
        {
            this.destinationPanel = destinationPanel;
            this.scriptFullpath = scriptFullpath;
            this.polaroidAnimator = new PolaroidAnimator(destinationPanel.RenderSize); // TODO: not instantiate here
            this.polaroidFactory = new PolaroidFactory(destinationPanel.RenderSize);  // TODO: not instantiate here
            this.soundPlayer = new SoundPlayer();
            this.scriptReader = new ScriptReader();
        }

        public async void PlayAndExit()
        {
            DiaporamaScript script = this.scriptReader.ReadFromFile(scriptFullpath);
            var fadeoutAnimator = new PanelChildFadoutAnimator(this.destinationPanel, script.FadeoutDuration);
            var polaroidRegistrer = new LayoutRegistrer<PolaroidUserControl>(script.MaximumPicturesPerLayout);

#if !DEBUG
            string songFullPath = Path.Combine(script.DataFolder, script.SongFilename);
            this.soundPlayer.PlayIfSet(songFullPath);
#endif

            await Task.Delay(script.StartTemporisationDuration);

            foreach (var currentStep in script.Steps)
            {
                PolaroidUserControl newPolaroid = await PlayStepAsync(script, currentStep);

                var polaroidToRemove = polaroidRegistrer.RegisterNewAndGetOldestElement(newPolaroid, currentStep.FinalLayout);
                if (polaroidToRemove != null)
                {
                    fadeoutAnimator.FadeOutAndRemove(polaroidToRemove);
                }
            }

            await Task.Delay(script.FinalFadeoutAfterLastStepDelay);

            FadoutBackground(script.FinalFadeoutAfterLastStepDuration);

            await soundPlayer.WaitUntilFinishedAsync();

            Exit();
        }

        private void FadoutBackground(TimeSpan duration)
        {
            var panelFadout = new FadeoutAnimator(this.destinationPanel, duration);
            panelFadout.Fadeout();
        }

        private static void Exit() => System.Windows.Application.Current.Shutdown();

        private async Task<PolaroidUserControl> PlayStepAsync(DiaporamaScript script, DiaporamaStep currentStep)
        {
            float pictureMarginRatio = currentStep.FinalLayout == FinalLayout.Full ? script.BigPictureMarginRatio : script.SmallPictureMarginRatio;
            string pictureFullPath = Path.Combine(script.DataFolder, currentStep.Filename);

            PolaroidUserControl polaroid = CreatePolaroid(polaroidFactory, pictureFullPath, pictureMarginRatio);

            polaroidAnimator.Animate(currentStep, polaroid, script.MaximumAbsoluteStartAngleDeviation, script.MaximumAbsoluteEndAngleDeviation);

            await Task.Delay(currentStep.Duration);

            return polaroid;
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
