﻿using System;
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

        public void Fadeout()
        {
            DoubleAnimation doubleAnimation = CreateOpacityAnimation(this.panel);

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
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
