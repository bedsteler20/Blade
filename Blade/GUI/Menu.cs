using Blade.Extensions;
using SkiaSharp;

namespace Blade.GUI;

public class Menu : Screen {
    private readonly string Title;
    private readonly SKPaint BackgroundPaint;

    const int ITEM_PADDING = 10;
    const int ITEM_HEIGHT = 30;
    const int ITEM_WIDTH = 200;
    const int EDGE_PADDING = 40;
    const int TOP_PADDING = 40;
    const int BOTTOM_PADDING = 40;
    private readonly int Height = 0;

    private readonly int Width = 0;

    public Menu(string title, Button[] buttons, SKColor backgroundColor) {
        Title = title;
        BackgroundPaint = new() {
            Color = backgroundColor
        };
        Height = ITEM_HEIGHT * buttons.Length + (buttons.Length - 1) * ITEM_PADDING + TOP_PADDING + BOTTOM_PADDING;
        Width = ITEM_WIDTH + (EDGE_PADDING * 2);
        foreach (var button in buttons) {
            button.Width = ITEM_WIDTH;
            button.Height = ITEM_HEIGHT;
        };
        Children.AddRange(buttons);
    }

    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.CenterScreen(Width, Height);
        canvas.DrawRect(0, 0, Width, Height, BackgroundPaint);
        int y = BOTTOM_PADDING;
        foreach (var child in Children) {
            if (child is Button button) {
                canvas.DrawDrawable(button, EDGE_PADDING, y);
                y += ITEM_HEIGHT + ITEM_PADDING;
            }
        }
    }
}
