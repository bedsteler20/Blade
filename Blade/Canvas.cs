using SkiaSharp;

namespace Blade.Extensions;

public static class CanvasExtension {
    public static void Center(this SKCanvas canvas, float x, float y) {
        canvas.Translate((Window.CurrentWindow.Width - x) / 2, (Window.CurrentWindow.Height - y) / 2);
    }
}