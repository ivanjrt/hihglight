using System.Windows;
using System.Windows.Media;

namespace ImageEditor.Models
{
    public class RectangleShape : DrawingShape
    {
        public RectangleShape(Point start, Point end)
        {
            var rect = new Rect(start, end);
            Position = rect.TopLeft;
            Size = new Size(rect.Width, rect.Height);
        }

        public RectangleShape(Rect rect)
        {
            Position = rect.TopLeft;
            Size = new Size(rect.Width, rect.Height);
        }

        public override void Render(DrawingContext context)
        {
            var rect = new Rect(Position, Size);
            var pen = new Pen(StrokeColor, StrokeThickness);
            context.DrawRectangle(FillColor, pen, rect);
        }
    }
}
