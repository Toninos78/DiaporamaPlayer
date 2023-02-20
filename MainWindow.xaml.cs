using System;
using System.Threading.Tasks;
using System.Windows;

namespace DiaporamaPlayer
{
    public partial class MainWindow : Window
    {
        private readonly DiaporamaConfiguration configuration = new DiaporamaConfiguration();

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            AnimateAsync();
        }

        private async void AnimateAsync()
        {
            await Task.Delay(2000);

            var allSteps = new DiaporamaStep[]
            {
                new DiaporamaStep("1.jpg", TimeSpan.FromSeconds(7), FinalLayout.Full, StepSource.TopLeft),
                new DiaporamaStep("2.jpg", TimeSpan.FromSeconds(7), FinalLayout.Left, StepSource.TopRight),
                new DiaporamaStep("3.jpg", TimeSpan.FromSeconds(7), FinalLayout.Right, StepSource.BottomRight),
                new DiaporamaStep("4.jpg", TimeSpan.FromSeconds(7), FinalLayout.Full, StepSource.BottomLeft),
                new DiaporamaStep("5.jpg", TimeSpan.FromSeconds(7), FinalLayout.Full, StepSource.Random),
            };

            var polaroidAnimator = new PolaroidAnimator(RenderSize, configuration);
            var polaroidFactory = new PolaroidFactory(configuration, RenderSize);

            foreach (var currentStep in allSteps)
            {
                PolaroidUserControl polaroid = CreatePolaroid(polaroidFactory, currentStep);

                polaroidAnimator.Animate(currentStep, polaroid);

                await Task.Delay(currentStep.Duration);
            }
        }

        private PolaroidUserControl CreatePolaroid(PolaroidFactory factory, DiaporamaStep currentStep)
        {
            PolaroidUserControl polaroid = factory.CreatePolaroid(currentStep.Filename, currentStep.FinalLayout);
            canvas.Children.Add(polaroid);

            UpdateLayout();

            return polaroid;
        }
    }
}
