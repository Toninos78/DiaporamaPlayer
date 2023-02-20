using System;
using System.Windows;

namespace DiaporamaPlayer
{
    internal class PolaroidAnimator
    {
        private Animator animator = new Animator(); // TODO: a injecter au constructeur
        private readonly static Random random = new Random();
        private readonly Size screenSize;

        public PolaroidAnimator(Size screenSize)
        {
            this.screenSize = screenSize;
        }

        public void Animate(DiaporamaStep diaporamaStep, PolaroidUserControl polaroid, float maximumAbsoluteStartAngleDeviation, float maximumAbsoluteEndAngleDeviation)
        {
            double startAngle = 2 * maximumAbsoluteStartAngleDeviation * (random.NextDouble() - 0.5);
            double endAngle = 2 * maximumAbsoluteEndAngleDeviation * (random.NextDouble() - 0.5);
            Point startPosition = GetStartPosition(diaporamaStep.Source, polaroid.RenderSize);
            Point endPosition = GetEndPosition(polaroid, diaporamaStep.FinalLayout);

            animator.StartAnimation(polaroid, startPosition, endPosition, startAngle, endAngle, diaporamaStep.Duration);
        }

        private Point GetStartPosition(StepSource stepSource, Size animatedElementSize)
        {
            StepSource actualStepSource = RandomizeStepSourceIfNecessary(stepSource);

            return actualStepSource switch
            {
                StepSource.TopLeft => new Point(-animatedElementSize.Width, -animatedElementSize.Height),
                StepSource.TopRight => new Point(screenSize.Width, -animatedElementSize.Height),
                StepSource.BottomRight => new Point(screenSize.Width, screenSize.Height),
                StepSource.BottomLeft => new Point(-animatedElementSize.Width, screenSize.Height),
                _ => throw new NotImplementedException(),
            };
        }

        private static StepSource RandomizeStepSourceIfNecessary(StepSource stepSource)
        {
            return stepSource == StepSource.Random ? (StepSource)random.Next(4) : stepSource;
        }

        private Point GetEndPosition(FrameworkElement frameworkElement, FinalLayout finalLayout)
        {
            double y = (screenSize.Height - frameworkElement.ActualHeight) / 2;
            double xCenter = GetEndPositionXCenter(finalLayout);

            return new Point(xCenter - frameworkElement.ActualWidth / 2, y);
        }

        private double GetEndPositionXCenter(FinalLayout layout)
        {
            return layout switch
            {
                FinalLayout.Full => screenSize.Width / 2,
                FinalLayout.Left => screenSize.Width / 5,
                FinalLayout.Right => 4 * screenSize.Width / 5,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
