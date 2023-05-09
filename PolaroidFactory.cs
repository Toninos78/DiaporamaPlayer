using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiaporamaPlayer
{
    // TODO: uniformize Picture / Image
    internal class PolaroidFactory
    {
        private readonly Size screenSize;

        public PolaroidFactory(Size screenSize)
        {
            this.screenSize = screenSize;
        }

        public PolaroidUserControl CreatePolaroid(string imageFullPath, double pictureMarginRatio)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageFullPath));
            RenderOptions.SetBitmapScalingMode(bitmapImage, BitmapScalingMode.LowQuality);

            var polaroid = new PolaroidUserControl() { PhotoSource = bitmapImage };
            double size = ComputePolaroidSize(pictureMarginRatio);

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

        private double ComputePolaroidSize(double pictureMarginRatio) => screenSize.Height * (1 - 2 * pictureMarginRatio);
    }
}
