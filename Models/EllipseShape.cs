using System.Windows;
using System.Windows.Media;

namespace ImageEditor.Models
{
    public class EllipseShape : DrawingShape
    {
        public EllipseShape(Point start, Point end)
        {
            var rect = new Rect(start, end);
            Position = rect.TopLeft;
            Size = new Size(rect.Width, rect.Height);
        }

        public EllipseShape(Rect rect)
        {
            Position = rect.TopLeft;
            Size = new Size(rect.Width, rect.Height);
        }

        public override void Render(DrawingContext context)
        {
            var ellipseRect = new Rect(Position, Size);
            var pen = new Pen(StrokeColor, StrokeThickness);
            context.DrawEllipse(FillColor, pen, new Point(
                ellipseRect.X + ellipseRect.Width / 2,
                ellipseRect.Y + ellipseRect.Height / 2
            ), ellipseRect.Width / 2, ellipseRect.Height / 2);
        }
    }
}
