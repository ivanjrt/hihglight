using System;
using System.Windows;
using System.Windows.Media;

namespace ImageEditor.Models
{
    public class ArrowShape : DrawingShape
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public double ArrowHeadSize { get; set; } = 15;

        public ArrowShape(Point start, Point end)
        {
            Start = start;
            End = end;

            // Calculate bounding box for the arrow
            var minX = Math.Min(start.X, end.X);
            var minY = Math.Min(start.Y, end.Y);
            var maxX = Math.Max(start.X, end.X);
            var maxY = Math.Max(start.Y, end.Y);

            Position = new Point(minX, minY);
            Size = new Size(maxX - minX, maxY - minY);
        }

        public override void Render(DrawingContext context)
        {
            var pen = new Pen(StrokeColor, StrokeThickness);

            // Draw the main line
            context.DrawLine(pen, Start, End);

            // Calculate arrow head
            var angle = Math.Atan2(End.Y - Start.Y, End.X - Start.X);
            var arrowAngle = Math.PI / 6; // 30 degrees

            // Left wing of arrowhead
            var leftWing = new Point(
                End.X - ArrowHeadSize * Math.Cos(angle - arrowAngle),
                End.Y - ArrowHeadSize * Math.Sin(angle - arrowAngle)
            );

            // Right wing of arrowhead
            var rightWing = new Point(
                End.X - ArrowHeadSize * Math.Cos(angle + arrowAngle),
                End.Y - ArrowHeadSize * Math.Sin(angle + arrowAngle)
            );

            // Draw arrowhead
            var arrowHeadGeometry = new StreamGeometry();
            using (var context2 = arrowHeadGeometry.Open())
            {
                context2.BeginFigure(End, true, true);
                context2.LineTo(leftWing, true, false);
                context2.LineTo(rightWing, true, false);
            }

            context.DrawGeometry(StrokeColor, null, arrowHeadGeometry);
        }
    }
}
