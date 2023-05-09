using System.Windows;
using System.Windows.Input;

namespace DiaporamaPlayer
{
    public partial class MainWindow : Window
    {
        private bool isDiaporamaStarted;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnKeyDown(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Space)
            {
                if (isDiaporamaStarted)
                {
                    return;
                }

                chevron.Visibility = Visibility.Visible;

                isDiaporamaStarted = true;
                Mouse.OverrideCursor = Cursors.None;

                DiaporamaConfiguration configuration = new DiaporamaConfiguration();
                var player = new Player(canvas, configuration.ScriptFullpath);
                player.PlayAndExit();
            }
            else if (keyEventArgs.Key == Key.Escape)
            {
                if (isDiaporamaStarted)
                {
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
