using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageEditor.Views
{
    public partial class TextDialog : Window
    {
        public string Text { get; private set; }
        public double SelectedFontSize { get; private set; }
        public Brush Color { get; private set; }

        public TextDialog(string initialText = "", double initialFontSize = 16)
        {
            InitializeComponent();
            txtTextInput.Text = initialText;
            txtTextInput.Focus();
            txtTextInput.Select(initialText.Length, 0);

            // Set the initial font size from the slider
            sliderFontSize.Value = initialFontSize;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Text = txtTextInput.Text;
            SelectedFontSize = sliderFontSize.Value;

            var selectedColor = ((ComboBoxItem)cmbColor.SelectedItem).Tag.ToString();
            Color = new BrushConverter().ConvertFromString(selectedColor) as Brush;

            if (string.IsNullOrWhiteSpace(Text))
            {
                MessageBox.Show("Please enter some text.", "Text Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
