using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageEditor.Models
{
    public class BlurShape : DrawingShape
    {
        public BitmapSource SourceImage { get; set; }
        public int PixelSize { get; set; } = 10;

        public BlurShape(BitmapSource source, Rect bounds)
        {
            Position = bounds.TopLeft;
            Size = new Size(bounds.Width, bounds.Height);
            SourceImage = source;
        }

        public override void Render(DrawingContext context)
        {
            if (SourceImage == null || Size.Width <= 0 || Size.Height <= 0)
                return;

            var blurredBitmap = CreateBlurBitmap();
            if (blurredBitmap != null)
            {
                context.DrawImage(blurredBitmap, new Rect(Position, Size));
            }
        }

        private BitmapSource CreateBlurBitmap()
        {
            try
            {
                // Calculate the actual bounds within the source image
                var sourceRect = new Rect(Position, Size);

                // Create a cropped version of the source
                var cropped = new CroppedBitmap(SourceImage, new Int32Rect(
                    (int)sourceRect.X,
                    (int)sourceRect.Y,
                    Math.Min((int)sourceRect.Width, SourceImage.PixelWidth - (int)sourceRect.X),
                    Math.Min((int)sourceRect.Height, SourceImage.PixelHeight - (int)sourceRect.Y)
                ));

                // Apply strong pixelation effect by downsampling and upsampling
                // Scale down to a very small size for dramatic blur
                var scaleFactor = Math.Max(1, Math.Min(Size.Width, Size.Height) / 15.0);
                var smallWidth = Math.Max(3, (int)(Size.Width / scaleFactor));
                var smallHeight = Math.Max(3, (int)(Size.Height / scaleFactor));

                var transformGroup = new TransformGroup();
                transformGroup.Children.Add(new ScaleTransform(smallWidth / Size.Width, smallHeight / Size.Height));

                var smallBitmap = new TransformedBitmap(cropped, transformGroup);

                // Scale back up with nearest neighbor for pixelation effect
                var upscaleTransform = new ScaleTransform(Size.Width / smallWidth, Size.Height / smallHeight);
                var pixelatedBitmap = new TransformedBitmap(smallBitmap, upscaleTransform);

                return pixelatedBitmap;
            }
            catch
            {
                return null;
            }
        }
    }
}
