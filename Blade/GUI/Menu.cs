using Blade.Extensions;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;

namespace Blade.GUI;

public class Menu : Screen {
    const int ITEM_PADDING = 10;
    const int ITEM_HEIGHT = 30;
    const int ITEM_WIDTH = 200;
    const int EDGE_PADDING = 30;
    const int TOP_PADDING = 40;
    const int BOTTOM_PADDING = 30;
    const int BORDER_RADIUS = 15;
    private readonly SKPaint BackgroundPaint = new();

    public required SKColor BackgroundColor {
        get => BackgroundPaint.Color;
        set => BackgroundPaint.Color = value;
    }
    public required Button[] Buttons {
        get => Children.Where(v => v is Button).Cast<Button>().ToArray();
        set {
            foreach (var button in value) {
                button.Width = ITEM_WIDTH;
                button.Height = ITEM_HEIGHT;
            };
            Children.Clear();
            Children.AddRange(value);
            value[0].IsFocused = true;
        }
    }
    public required string Title { get; init; }
    private int Height => ITEM_HEIGHT * Buttons.Length + (Buttons.Length - 1) * ITEM_PADDING + TOP_PADDING + BOTTOM_PADDING;
    private readonly int Width = ITEM_WIDTH + (EDGE_PADDING * 2);

    private int _selectedIndex = 0;
    private int SelectedIndex {
        get => _selectedIndex;
        set {
            Buttons[_selectedIndex].IsFocused = false;
            if (value < 0) {
                value = Buttons.Length - 1;
            } else if (value >= Buttons.Length) {
                value = 0;
            }
            _selectedIndex = value;
            Buttons[_selectedIndex].IsFocused = true;
        }
    }


    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.CenterScreen(Width, Height);
        canvas.DrawRoundRect(0, 0, Width, Height, BORDER_RADIUS, BORDER_RADIUS, BackgroundPaint);
        int y = BOTTOM_PADDING;
        foreach (var button in Buttons) {
            canvas.DrawDrawable(button, EDGE_PADDING, y);
            y += ITEM_HEIGHT + ITEM_PADDING;
        }
    }

    public override void OnKeyDown(KeyboardKeyEventArgs e) {
        base.OnKeyDown(e);

        switch (e.Key) {
            case Keys.Up:
                SelectedIndex--;
                break;
            case Keys.Down:
                SelectedIndex++;
                break;
            case Keys.Enter:
                Buttons[SelectedIndex].Activate();
                break;
            case Keys.Escape:
                if (ScreenManager.Count > 1) ScreenManager.Back();
                break;
        }
    }
}
