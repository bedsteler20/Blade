using Blade.Colors;
using SkiaSharp;
using Blade;
namespace Blade.GUI;

public class Button : GameObject
{
    private readonly string Text;
    private readonly Action OnClick;
    public int Width { get; set; } = 0;
    public int Height { get; set; } = 0;
    public bool IsFocused { get; set; } = false;

    private readonly SKPaint BackgroundPaint = new()
    {
        Color = Catppuccin.Overlay1,
    };
    private readonly SKPaint TextPaint = new()
    {
        Color = Catppuccin.Surface1,
        TextAlign = SKTextAlign.Center,
        IsAntialias = true,
        TextSize = 24
    };

    protected override SKRect OnGetBounds() => new(Width, 0, 0, Height);


    private readonly SKPaint BackgroundSelectedPaint = new()
    {
        Color = Catppuccin.Surface1,
    };
    private readonly SKPaint TextSelectedPaint = new()
    {
        Color = Catppuccin.Overlay1,
        TextAlign = SKTextAlign.Center,
        IsAntialias = true,
        TextSize = 24
    };

    public Button(string text, Action onClick, int width, int height)
    {
        Text = text;
        OnClick = onClick;
        Width = width;
        Height = height;
    }

    public Button(string text, Action onClick)
    {
        Text = text;
        OnClick = onClick;
    }

    public void Activate() => OnClick();

    protected override void OnDraw(SKCanvas canvas)
    {
        base.OnDraw(canvas);
        canvas.DrawRect(0, 0, Width, Height, BackgroundPaint);
        canvas.DrawText(Text, Bounds.MidX, Bounds.MidY + TextPaint.TextSize / ((Height - TextPaint.TextSize) / 2), TextPaint);
    }

    protected override void Dispose(bool disposing)
    {
        BackgroundPaint.Dispose();
        TextPaint.Dispose();
        BackgroundSelectedPaint.Dispose();
        TextSelectedPaint.Dispose();
        base.Dispose(disposing);
    }
}
