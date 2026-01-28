# ✨ Image Editor 1.0.0

<img width="889" height="546" alt="image" src="https://github.com/user-attachments/assets/12d5d63d-9658-4e61-8cba-ad62118d7147" />


A Simple modern **WPF-based image editing application** with a sleek dark theme, designed for quick image annotations and edits, 
without having to save Personal information directly to the computer, just manipulating the clipboard or already saved files

![Version](https://img.shields.io/badge/version-1.0.0-blue)
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
- **✏️ Text** - Add text annotations directly on the canvas

### ⚙️ Customization
- **🎨 Color Picker** - Choose from 8 colors (Black, Red, Blue, Green, Yellow, Orange, Purple, White)
- **📏 Size Slider** - Adjust stroke thickness (1-10)

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
1. **Open an image** - Click "📁 Upload" or "📋 Paste"
2. **Select a tool** - Click any drawing tool button
3. **Draw on the canvas** - Click and drag to create shapes
4. **Customize** - Adjust color and size before drawing
5. **Export** - Click "📤 Copy" to copy to clipboard
6. **Undo mistakes** - Click "↩️ Undo" to remove last action

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
└── Converters/
    └── EnumToBooleanConverter.cs  # Data binding converters
```

## 🎨 UI Features

- ✨ Modern dark theme (#1E1E1E background)
- 🎯 Emoji-enhanced button labels
- 🖱️ Smooth hover effects
- 📍 Active tool highlighting (steel blue)
- 📱 Responsive layout with two-row toolbar

## 🔜 Future Enhancements (Optional)

- Undo/Redo history (multiple levels)
- Save image to file
- Move/resize existing shapes
- Zoom and pan controls
- Multiple layers support
- Additional shapes (lines, freehand)
- Keyboard shortcuts

## 📝 License

This is a personal project for image editing and annotation purposes.

---

**Made with ❤️ using WPF & C#**

*For support or questions, please refer to the project documentation.*
