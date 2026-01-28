using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ImageEditor.Models;

namespace ImageEditor
{
    public enum ToolType
    {
        Select,
        Blur,
        Highlight,
        Circle,
        Rectangle,
        Arrow,
        Text
    }

    public partial class MainWindow : Window
    {
        private ToolType _currentTool = ToolType.Select;
        private Point _startPoint;
        private bool _isDrawing = false;
        private System.Windows.Media.Imaging.BitmapSource _sourceImage;
        private List<DrawingShape> _shapes = new List<DrawingShape>();
        private Rectangle _previewRectangle;
        private Brush _currentColor = Brushes.Red;
        private double _strokeThickness = 2;
        private TextBox _activeTextBox = null;
        private Button _activeToolButton = null;

        public MainWindow()
        {
            InitializeComponent();
            cmbColor.SelectionChanged += CmbColor_SelectionChanged;
            sliderThickness.ValueChanged += SliderThickness_ValueChanged;
        }

        private void CmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbColor.SelectedItem is ComboBoxItem item)
            {
                var colorTag = item.Tag.ToString();
                _currentColor = new BrushConverter().ConvertFromString(colorTag) as Brush;
            }
        }

        private void SliderThickness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _strokeThickness = sliderThickness.Value;
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png|All Files|*.*",
                Title = "Open Image"
            };

            if (dialog.ShowDialog() == true)
            {
                LoadImage(dialog.FileName);
            }
        }

        private void LoadImage(string path)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(path);
                bitmap.EndInit();

                SetSourceImage(bitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPaste_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                try
                {
                    var bitmap = Clipboard.GetImage();
                    if (bitmap != null)
                    {
                        SetSourceImage(bitmap);
                    }
                    else
                    {
                        MessageBox.Show("No valid image found on clipboard.", "Paste Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error pasting from clipboard: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No image found on clipboard.", "Paste Failed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SetSourceImage(System.Windows.Media.Imaging.BitmapSource bitmap)
        {
            _sourceImage = bitmap;

            // Clear canvas and add image
            mainCanvas.Children.Clear();
            var image = new Image
            {
                Source = bitmap,
                Width = bitmap.PixelWidth,
                Height = bitmap.PixelHeight,
                Stretch = Stretch.None
            };
            mainCanvas.Children.Add(image);
            mainCanvas.Width = bitmap.PixelWidth;
            mainCanvas.Height = bitmap.PixelHeight;

            _shapes.Clear();

            // Hide empty state message
            txtEmptyState.Visibility = Visibility.Collapsed;

            // Enable buttons
            btnBlur.IsEnabled = true;
            btnHighlight.IsEnabled = true;
            btnCircle.IsEnabled = true;
            btnRectangle.IsEnabled = true;
            btnArrow.IsEnabled = true;
            btnAddText.IsEnabled = true;
            btnFitToScreen.IsEnabled = true;
            btnActualSize.IsEnabled = true;
            btnCopyToClipboard.IsEnabled = true;
            btnReset.IsEnabled = true;
            UpdateUndoButton();

            // Auto fit to screen
            Dispatcher.BeginInvoke(new Action(() =>
            {
                BtnFitToScreen_Click(null, null);
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        private void BtnFitToScreen_Click(object sender, RoutedEventArgs e)
        {
            if (_sourceImage == null) return;

            // Get the scroll viewer's actual available space
            double availableWidth = scrollViewer.ActualWidth - 10;
            double availableHeight = scrollViewer.ActualHeight - 10;

            // If scroll viewer hasn't been measured yet, use reasonable defaults
            if (availableWidth < 100) availableWidth = 850;
            if (availableHeight < 100) availableHeight = 550;

            // Calculate scale to fit
            double scaleX = availableWidth / _sourceImage.PixelWidth;
            double scaleY = availableHeight / _sourceImage.PixelHeight;
            double scale = Math.Min(scaleX, scaleY);

            // Apply scale to canvas
            mainCanvas.LayoutTransform = new ScaleTransform(scale, scale);
            mainCanvas.Width = _sourceImage.PixelWidth;
            mainCanvas.Height = _sourceImage.PixelHeight;
        }

        private void BtnActualSize_Click(object sender, RoutedEventArgs e)
        {
            // Reset to actual size
            mainCanvas.LayoutTransform = null;
            if (_sourceImage != null)
            {
                mainCanvas.Width = _sourceImage.PixelWidth;
                mainCanvas.Height = _sourceImage.PixelHeight;
            }
        }

        private void BtnBlur_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTool((Button)sender, ToolType.Blur);
        }

        private void SetActiveTool(Button button, ToolType tool)
        {
            // Don't do anything if clicking the same button
            if (_activeToolButton == button)
                return;

            _currentTool = tool;

            // Reset previous active button
            if (_activeToolButton != null)
            {
                _activeToolButton.ClearValue(Button.BackgroundProperty);
                _activeToolButton.ClearValue(Button.BorderBrushProperty);
            }

            // Set new active button with modern highlight
            _activeToolButton = button;
            _activeToolButton.Background = new SolidColorBrush(Color.FromRgb(70, 130, 180)); // Steel blue highlight
            _activeToolButton.BorderBrush = new SolidColorBrush(Color.FromRgb(100, 180, 220)); // Lighter border

            // Set cursor
            switch (tool)
            {
                case ToolType.Text:
                    mainCanvas.Cursor = Cursors.IBeam;
                    break;
                default:
                    mainCanvas.Cursor = Cursors.Cross;
                    break;
            }
        }

        private void BtnHighlight_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTool((Button)sender, ToolType.Highlight);
        }

        private void BtnCircle_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTool((Button)sender, ToolType.Circle);
        }

        private void BtnRectangle_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTool((Button)sender, ToolType.Rectangle);
        }

        private void BtnArrow_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTool((Button)sender, ToolType.Arrow);
        }

        private void BtnAddText_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTool((Button)sender, ToolType.Text);
        }

        private void BtnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (_sourceImage == null)
            {
                MessageBox.Show("Please upload an image first.", "No Image", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var finalBitmap = new RenderTargetBitmap(
                    (int)_sourceImage.PixelWidth,
                    (int)_sourceImage.PixelHeight,
                    96, 96,
                    PixelFormats.Pbgra32
                );

                var drawingVisual = new DrawingVisual();
                using (var context = drawingVisual.RenderOpen())
                {
                    // Draw the base image
                    context.DrawImage(_sourceImage, new Rect(0, 0, _sourceImage.PixelWidth, _sourceImage.PixelHeight));

                    // Draw all shapes
                    foreach (var shape in _shapes)
                    {
                        shape.Render(context);
                    }
                }

                finalBitmap.Render(drawingVisual);
                Clipboard.SetImage(finalBitmap);
                MessageBox.Show("Image copied to clipboard!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying to clipboard: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (_shapes.Count > 0 && MessageBox.Show("Clear all annotations?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _shapes.Clear();
                RenderShapes();
                UpdateUndoButton();
            }
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (_shapes.Count > 0)
            {
                _shapes.RemoveAt(_shapes.Count - 1);
                RenderShapes();
                UpdateUndoButton();
            }
        }

        private void UpdateUndoButton()
        {
            btnUndo.IsEnabled = _shapes.Count > 0 && _sourceImage != null;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_sourceImage == null)
            {
                MessageBox.Show("Please upload an image first.", "No Image", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _startPoint = e.GetPosition(mainCanvas);
            _isDrawing = true;

            if (_currentTool == ToolType.Text)
            {
                HandleTextInput(_startPoint);
                _isDrawing = false;
            }
            else
            {
                CreatePreview(_startPoint);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing || _previewRectangle == null)
                return;

            var currentPoint = e.GetPosition(mainCanvas);
            UpdatePreview(currentPoint);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isDrawing)
                return;

            var endPoint = e.GetPosition(mainCanvas);

            if (_currentTool != ToolType.Text)
            {
                FinalizeShape(endPoint);
            }

            _isDrawing = false;

            if (_previewRectangle != null)
            {
                mainCanvas.Children.Remove(_previewRectangle);
                _previewRectangle = null;
            }
        }

        private void CreatePreview(Point point)
        {
            _previewRectangle = new Rectangle
            {
                Stroke = Brushes.Gray,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection(new[] { 4.0, 4.0 }),
                Fill = Brushes.Transparent
            };

            Canvas.SetLeft(_previewRectangle, point.X);
            Canvas.SetTop(_previewRectangle, point.Y);
            mainCanvas.Children.Add(_previewRectangle);
        }

        private void UpdatePreview(Point currentPoint)
        {
            if (_previewRectangle == null) return;

            var rect = new Rect(_startPoint, currentPoint);
            Canvas.SetLeft(_previewRectangle, rect.X);
            Canvas.SetTop(_previewRectangle, rect.Y);
            _previewRectangle.Width = rect.Width;
            _previewRectangle.Height = rect.Height;
        }

        private void FinalizeShape(Point endPoint)
        {
            var rect = new Rect(_startPoint, endPoint);

            // Ignore tiny shapes
            if (rect.Width < 5 && rect.Height < 5)
                return;

            DrawingShape shape = null;

            switch (_currentTool)
            {
                case ToolType.Blur:
                    shape = new BlurShape(_sourceImage, rect);
                    break;

                case ToolType.Highlight:
                    shape = new HighlightShape(rect, Colors.Yellow, 0.4);
                    break;

                case ToolType.Circle:
                    shape = new EllipseShape(rect)
                    {
                        StrokeColor = _currentColor,
                        StrokeThickness = _strokeThickness
                    };
                    break;

                case ToolType.Rectangle:
                    shape = new RectangleShape(rect)
                    {
                        StrokeColor = _currentColor,
                        StrokeThickness = _strokeThickness
                    };
                    break;

                case ToolType.Arrow:
                    shape = new ArrowShape(_startPoint, endPoint)
                    {
                        StrokeColor = _currentColor,
                        StrokeThickness = _strokeThickness
                    };
                    break;
            }

            if (shape != null)
            {
                _shapes.Add(shape);
                RenderShapes();
                UpdateUndoButton();
            }
        }

        private void HandleTextInput(Point position)
        {
            // If there's already an active text box, finalize it first
            if (_activeTextBox != null)
            {
                FinalizeActiveTextBox();
                return;
            }

            // Calculate font size from slider (1-10 becomes 12-48)
            double fontSize = _strokeThickness * 4 + 8;

            // Create a text box directly on the canvas
            _activeTextBox = new TextBox
            {
                Text = "",
                FontSize = fontSize,
                Foreground = _currentColor,
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Width = 200,
                Height = fontSize + 10,
                AcceptsReturn = false
            };

            // Position the text box
            Canvas.SetLeft(_activeTextBox, position.X);
            Canvas.SetTop(_activeTextBox, position.Y);

            // Handle key events
            _activeTextBox.KeyDown += (s, ke) =>
            {
                if (ke.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.None)
                {
                    FinalizeActiveTextBox();
                    ke.Handled = true;
                }
                else if (ke.Key == Key.Escape)
                {
                    CancelActiveTextBox();
                    ke.Handled = true;
                }
            };

            // Handle lost focus to finalize (with delay to allow clicking elsewhere)
            _activeTextBox.LostFocus += (s, e) =>
            {
                // Use a small delay to avoid immediate finalization
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (_activeTextBox != null && !_activeTextBox.IsKeyboardFocused)
                    {
                        FinalizeActiveTextBox();
                    }
                }), System.Windows.Threading.DispatcherPriority.Input);
            };

            // Handle mouse click to capture focus
            _activeTextBox.MouseLeftButtonDown += (s, e) =>
            {
                e.Handled = true;
            };

            mainCanvas.Children.Add(_activeTextBox);

            // Focus the text box with a slight delay to ensure it's rendered
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _activeTextBox.Focus();
                _activeTextBox.CaretIndex = 0;
            }), System.Windows.Threading.DispatcherPriority.Input);
        }

        private void FinalizeActiveTextBox()
        {
            if (_activeTextBox == null) return;

            string text = _activeTextBox.Text;
            double fontSize = _activeTextBox.FontSize;
            Brush color = _activeTextBox.Foreground;
            Point position = new Point(Canvas.GetLeft(_activeTextBox), Canvas.GetTop(_activeTextBox));

            mainCanvas.Children.Remove(_activeTextBox);
            _activeTextBox = null;

            if (!string.IsNullOrWhiteSpace(text))
            {
                var textShape = new TextShape(position, text, color, fontSize);
                _shapes.Add(textShape);
                RenderShapes();
                UpdateUndoButton();
            }
        }

        private void CancelActiveTextBox()
        {
            if (_activeTextBox == null) return;

            mainCanvas.Children.Remove(_activeTextBox);
            _activeTextBox = null;
        }

        private void RenderShapes()
        {
            if (_sourceImage == null)
                return;

            // Save active text box if exists
            TextBox savedTextBox = _activeTextBox;
            double savedTextBoxX = 0, savedTextBoxY = 0;
            if (savedTextBox != null)
            {
                savedTextBoxX = Canvas.GetLeft(savedTextBox);
                savedTextBoxY = Canvas.GetTop(savedTextBox);
            }

            // Clear existing shapes (keep the base image)
            mainCanvas.Children.Clear();

            // Re-add base image
            var image = new Image
            {
                Source = _sourceImage,
                Width = _sourceImage.PixelWidth,
                Height = _sourceImage.PixelHeight,
                Stretch = Stretch.None
            };
            mainCanvas.Children.Add(image);

            // Render all shapes
            var drawingVisual = new DrawingVisual();
            using (var context = drawingVisual.RenderOpen())
            {
                foreach (var shape in _shapes)
                {
                    shape.Render(context);
                }
            }

            var renderer = new RenderTargetBitmap(
                (int)_sourceImage.PixelWidth,
                (int)_sourceImage.PixelHeight,
                96, 96,
                PixelFormats.Pbgra32
            );
            renderer.Render(drawingVisual);

            var shapesImage = new Image
            {
                Source = renderer,
                Width = _sourceImage.PixelWidth,
                Height = _sourceImage.PixelHeight,
                Stretch = Stretch.None,
                IsHitTestVisible = false
            };
            mainCanvas.Children.Add(shapesImage);

            // Restore active text box if exists
            if (savedTextBox != null)
            {
                mainCanvas.Children.Add(savedTextBox);
                Canvas.SetLeft(savedTextBox, savedTextBoxX);
                Canvas.SetTop(savedTextBox, savedTextBoxY);
                savedTextBox.Focus();
            }
        }
    }
}
