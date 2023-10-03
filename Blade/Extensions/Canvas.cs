using SkiaSharp;

namespace Blade.Extensions;

public static class CanvasExtension {
    public static void CenterScreen(this SKCanvas canvas, float x, float y) {
        canvas.Translate((Window.CurrentWindow.Width - x) / 2, (Window.CurrentWindow.Height - y) / 2);
    }

    public static void Center(this SKCanvas canvas, SKDrawable drawable) {
        canvas.Translate((drawable.Bounds.Left + drawable.Bounds.Width) / 2,
                         (drawable.Bounds.Top + drawable.Bounds.Height) / 2);
    }
}