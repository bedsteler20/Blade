using Blade.Colors;
using SkiaSharp;
namespace Blade.GUI;

public class Button : GameObject {
    const int BORDER_RADIUS = 8;
    public required string Text { get; set; }
    public required Action OnClick { get; init; }
    public int Width { get; set; } = 0;
    public int Height { get; set; } = 0;
    public float TextHeight {
        get => TextPaint.TextSize;
        set {
            TextPaint.TextSize = value;
            TextSelectedPaint.TextSize = value;
        }
    }
    public bool IsFocused { get; set; } = false;

    private readonly SKPaint BackgroundPaint = new() {
        Color = Catppuccin.Overlay1,
    };

    private readonly SKPaint TextPaint = new() {
        Color = Catppuccin.Surface1,
        TextAlign = SKTextAlign.Center,
        IsAntialias = true,
        TextSize = 24,
        Typeface = Catppuccin.Font
    };

    private readonly SKPaint BackgroundSelectedPaint = new() {
        Color = Catppuccin.Surface1,
    };
    private readonly SKPaint TextSelectedPaint = new() {
        Color = Catppuccin.Text,
        TextAlign = SKTextAlign.Center,
        IsAntialias = true,
        TextSize = 24,
        Typeface = Catppuccin.Font
    };

    protected override SKRect OnGetBounds() => new(Width, 0, 0, Height);
    public void Activate() => OnClick();

    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        var BackgroundPaint = IsFocused ? BackgroundSelectedPaint : this.BackgroundPaint;
        var TextPaint = IsFocused ? TextSelectedPaint : this.TextPaint;
        canvas.DrawRoundRect(0, 0, Width, Height, BORDER_RADIUS, BORDER_RADIUS, BackgroundPaint);
        canvas.DrawText(Text, Bounds.MidX, Bounds.MidY + TextPaint.TextSize / ((Height - TextPaint.TextSize) / 2), TextPaint);
    }

    protected override void Dispose(bool disposing) {
        BackgroundPaint.Dispose();
        TextPaint.Dispose();
        BackgroundSelectedPaint.Dispose();
        TextSelectedPaint.Dispose();
        base.Dispose(disposing);
    }
}
