using System.Windows;
using System.Windows.Media;

namespace ImageEditor.Models
{
    public abstract class DrawingShape
    {
        public Point Position { get; set; }
        public Size Size { get; set; }
        public Brush StrokeColor { get; set; }
        public double StrokeThickness { get; set; }
        public Brush FillColor { get; set; }

        protected DrawingShape()
        {
            StrokeColor = Brushes.Red;
            StrokeThickness = 2;
            FillColor = Brushes.Transparent;
        }

        public abstract void Render(DrawingContext context);
    }
}
