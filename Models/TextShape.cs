using System.Windows;
using System.Windows.Media;

namespace ImageEditor.Models
{
    public class TextShape : DrawingShape
    {
        public string Text { get; set; }
        public double FontSize { get; set; } = 16;
        public FontFamily FontFamily { get; set; } = new FontFamily("Arial");
        public FontStyle FontStyle { get; set; } = FontStyles.Normal;
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;

        public TextShape(Point position, string text)
        {
            Position = position;
            Text = text;
            StrokeColor = Brushes.Black;
        }

        public TextShape(Point position, string text, Brush color, double fontSize)
        {
            Position = position;
            Text = text;
            StrokeColor = color;
            FontSize = fontSize;
        }

        public override void Render(DrawingContext context)
        {
            if (string.IsNullOrEmpty(Text))
                return;

            var formattedText = new FormattedText(
                Text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal),
                FontSize,
                StrokeColor
            );

            context.DrawText(formattedText, Position);
        }

        public Size GetTextSize()
        {
            if (string.IsNullOrEmpty(Text))
                return new Size(0, 0);

            var formattedText = new FormattedText(
                Text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal),
                FontSize,
                StrokeColor
            );

            return new Size(formattedText.Width, formattedText.Height);
        }
    }
}
