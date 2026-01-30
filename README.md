# ✨ Image Editor 1.0.2

<img width="902" height="675" alt="image" src="https://github.com/user-attachments/assets/00b6487d-2b4e-4d7c-b5df-ce6db0565901" />



A simple modern **WPF-based image editing application** with a sleek dark theme, designed for quick image annotations and edits,
without having to save personal information directly to the computer - just manipulate the clipboard or already saved files.

![Version](https://img.shields.io/badge/version-1.0.1-blue)
![Framework](https://img.shields.io/badge/.NET-4.0-purple)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)

## 🎯 Features

### 📥 Image Input
- **📁 Upload** - Load images from your computer (JPEG, PNG)
- **📋 Paste from Clipboard** - Quickly paste screenshots or copied images

### 🎨 Drawing Tools
- **🌫️ Blur** - Apply strong pixelation/blur effect to regions
- **🖍️ Highlight** - Add semi-transparent colored highlights
- **⭕ Circle/Ellipse** - Draw circular or elliptical shapes
- **⬜ Rectangle** - Draw rectangular shapes
- **➡️ Arrow** - Draw arrows with arrowheads
- **✏️ Text** - Add text annotations directly on the canvas (click and type!)

### ⌨️ Keyboard Shortcuts
- **Ctrl+C** - Copy image to clipboard
- **Ctrl+V** - Paste from clipboard
- **Ctrl+Z** - Undo last action
- **Escape** - Cancel text input

### 🖱️ Right-Click Menu
- Quick access to Copy, Paste, Undo, and Clear All
- Appears when right-clicking on the canvas

### ❓ Help Dialog
- Complete keyboard shortcuts reference
- Tool descriptions and what settings apply to each
- Quick start guide for new users
- Tips and tricks

### ⚙️ Customization
- **🎨 Color Picker** - 8 colors (Black, Red, Blue, Green, Yellow, Orange, Purple, White)
- **📏 Size Slider** - Adjustable stroke thickness (1-10)

   **Applies to:** Highlight, Circle, Rectangle, Arrow, Text
   **Does not apply to:** Blur (uses fixed pixelation strength)

### 🔧 Canvas Controls
- **🔍 Fit to Screen** - Automatically scale image to fit the viewport
- **1️⃣ 100%** - View image at actual size
- **↩️ Undo** - Remove the last action
- **📤 Copy to Clipboard** - Export edited image to clipboard
- **🗑️ Clear All** - Remove all annotations

## 🚀 Getting Started

### Requirements
- Windows OS
- .NET Framework 4.0 or higher

### How to Use
1. **Open an image** - Click "📁 Upload" or "📋 Paste" (or Ctrl+V)
2. **Select a tool** - Click any drawing tool button (highlighted when active)
3. **Draw on the canvas** - Click and drag to create shapes
4. **Customize** - Adjust color and size before drawing
5. **Text input** - Click Text tool, click on canvas, type, press Enter
6. **Export** - Click "📤 Copy" or press Ctrl+C
7. **Undo** - Click "↩️ Undo", press Ctrl+Z, or right-click canvas
8. **Get help** - Click "❓ Help" button for shortcuts and tips

## 💻 Technical Details

- **Language**: C# / WPF
- **Architecture**: MVVM-inspired with shape-based rendering
- **.NET Framework**: 4.0
- **IDE**: Visual Studio

## 📂 Project Structure

```
Highlight/
├── MainWindow.xaml           # Main UI with dark theme
├── MainWindow.xaml.cs        # Core application logic
├── Models/
│   ├── DrawingShape.cs       # Base shape class
│   ├── BlurShape.cs          # Blur/pixelation effect
│   ├── HighlightShape.cs     # Highlight rectangles
│   ├── EllipseShape.cs       # Circle/ellipse shapes
│   ├── RectangleShape.cs     # Rectangle shapes
│   ├── ArrowShape.cs         # Arrow shapes
│   └── TextShape.cs          # Text annotations
├── Views/
│   ├── HelpDialog.xaml       # Help dialog
│   └── HelpDialog.xaml.cs
└── Converters/
    └── EnumToBooleanConverter.cs  # Data binding converters
```

## 🎨 UI Features

- ✨ Modern dark theme (#1E1E1E background)
- 🎯 Emoji-enhanced button labels for quick recognition
- 🖱️ Smooth hover effects with rounded corners
- 📍 Active tool highlighting (steel blue, 70-130-180 RGB)
- 📱 Responsive two-row toolbar layout
- 🖱️ Right-click context menu for quick actions
- ⌨️ Full keyboard shortcut support

## 🔜 Future Enhancements (Optional)

- Multi-level undo/redo history
- Save image to file (currently clipboard only)
- Move/resize existing shapes after drawing
- Advanced zoom and pan controls
- Multiple layers support
- Additional shapes (lines, freehand drawing)
- More blur strength options

## 📝 License

This is a personal project for image editing and annotation purposes.

---

**Made with ❤️ using WPF & C#**

*For support or questions, please refer to the project documentation.*
