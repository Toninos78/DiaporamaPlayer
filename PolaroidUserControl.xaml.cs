using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DiaporamaPlayer
{
    /// <summary>
    /// Interaction logic for PolaroidUserControl.xaml
    /// </summary>
    public partial class PolaroidUserControl : UserControl
    {
        public PolaroidUserControl()
        {
            InitializeComponent();
        }



        public ImageSource PhotoSource
        {
            get { return (ImageSource)GetValue(PhotoSourceProperty); }
            set { SetValue(PhotoSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PhotoSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PhotoSourceProperty =
            DependencyProperty.Register("PhotoSource", typeof(ImageSource), typeof(PolaroidUserControl), new PropertyMetadata(null));
    }
}
