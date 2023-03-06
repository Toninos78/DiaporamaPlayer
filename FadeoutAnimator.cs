using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DiaporamaPlayer
{
    public class FadeoutAnimator
    {
        private readonly Panel panel;
        private readonly TimeSpan duration;

        public FadeoutAnimator(Panel panel, TimeSpan duration)
        {
            this.panel = panel;
            this.duration = duration;
        }

        public async void FadeOutAndRemove(UIElement controlToFadeout)
        {
            await FadeoutAsync(controlToFadeout);

            this.panel.Children.Remove(controlToFadeout);
        }

        private async Task FadeoutAsync(UIElement controlToFadeout)
        {
            DoubleAnimation doubleAnimation = CreateOpacityAnimation(controlToFadeout);

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();

            await Task.Delay(duration);
        }

        private DoubleAnimation CreateOpacityAnimation(UIElement controlToFadeout)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, duration);

            Storyboard.SetTarget(doubleAnimation, controlToFadeout);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(UIElement.OpacityProperty));
            return doubleAnimation;
        }
    }
}
