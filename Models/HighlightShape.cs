using System.Windows;
using System.Windows.Media;

namespace ImageEditor.Models
{
    public class HighlightShape : DrawingShape
    {
        public double Opacity { get; set; } = 0.4;

        public HighlightShape(Point start, Point end)
        {
            var rect = new Rect(start, end);
            Position = rect.TopLeft;
            Size = new Size(rect.Width, rect.Height);
            FillColor = new SolidColorBrush(Color.FromArgb(204, 255, 255, 0)); // Semi-transparent yellow
        }

        public HighlightShape(Rect rect)
        {
            Position = rect.TopLeft;
            Size = new Size(rect.Width, rect.Height);
            FillColor = new SolidColorBrush(Color.FromArgb(204, 255, 255, 0)); // Semi-transparent yellow
        }

        public HighlightShape(Rect rect, Color color, double opacity)
        {
            Position = rect.TopLeft;
            Size = new Size(rect.Width, rect.Height);
            FillColor = new SolidColorBrush(Color.FromArgb(
                (byte)(255 * opacity),
                color.R,
                color.G,
                color.B
            ));
        }

        public override void Render(DrawingContext context)
        {
            var rect = new Rect(Position, Size);
            context.DrawRectangle(FillColor, null, rect);
        }
    }
}
