using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DiaporamaPlayer
{
    internal class Animator
    {
        public void StartAnimation(FrameworkElement target, Point startPosition, Point endPosition, double startAngle, double endAngle, TimeSpan duration)
        {
            TranslateTransform translateTransform = new TranslateTransform();
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.CenterX = target.ActualWidth / 2;
            rotateTransform.CenterY = target.ActualHeight / 2;
            AddRenderTransform(target, new Transform[] { translateTransform, rotateTransform });
            DoubleAnimation animationX = CreateDoubleAnimation(startPosition.X, endPosition.X, duration);
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animationX);
            DoubleAnimation animationY = CreateDoubleAnimation(startPosition.Y, endPosition.Y, duration);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, animationY);
            DoubleAnimation rotation = CreateDoubleAnimation(startAngle, endAngle, duration);
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotation);
        }

        private static void AddRenderTransform(UIElement target, IEnumerable<Transform> allTransforms)
        {
            var transformGroup = new TransformGroup();
            foreach (var t in allTransforms)
            {
                transformGroup.Children.Add(t);
            }
            target.RenderTransform = transformGroup;
        }

        private static DoubleAnimation CreateDoubleAnimation(double from, double to, TimeSpan duration)
        {
            return new DoubleAnimation
            {
                From = from,
                To = to,
                EasingFunction = new QuadraticEase(),
                Duration = duration
            };
        }
    }
}
