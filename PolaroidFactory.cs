using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiaporamaPlayer
{
    internal class PolaroidFactory
    {
        private readonly DiaporamaConfiguration configuration;
        private readonly Size screenSize;

        public PolaroidFactory(DiaporamaConfiguration configuration, Size screenSize)
        {
            this.configuration = configuration;
            this.screenSize = screenSize;
        }

        public PolaroidUserControl CreatePolaroid(string currentImage, FinalLayout finalLayout)
        {
            string fullPath = Path.Combine(configuration.ImageFolder, currentImage);
            BitmapImage bitmapImage = new BitmapImage(new Uri(fullPath));
            RenderOptions.SetBitmapScalingMode(bitmapImage, BitmapScalingMode.LowQuality);

            var polaroid = new PolaroidUserControl() { PhotoSource = bitmapImage };
            double size = GetMostConstrainingDimensionSize(finalLayout);

            if (bitmapImage.PixelHeight > bitmapImage.PixelWidth)
            {
                polaroid.Height = size;
            }
            else
            {
                polaroid.Width = size;
            }

            return polaroid;
        }

        private double GetMostConstrainingDimensionSize(FinalLayout finalLayout)
        {
            var pictureMarginRatio = finalLayout == FinalLayout.Full ? configuration.BigPictureMarginRatio : configuration.SmallPictureMarginRatio;
            return screenSize.Height * (1 - 2 * pictureMarginRatio);
        }
    }
}
