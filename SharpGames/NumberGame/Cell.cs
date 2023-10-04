using SkiaSharp;
using Blade.Colors;
namespace SharpGames.NumberGame;

public class Cell : Blade.GameObject {
    public const int SIZE = 100;
    private const int TEXT_HEIGHT = 60;
    private const int TEXT_SIZE = 32;
    private const int BORDER_RADIUS = 10;
    private int _value;

    public int Value {
        get => _value;
        set {
            _value = value;
            BackgroundPaint.Color = BackgroundColor;
        }
    }

    protected override SKRect OnGetBounds() {
        return new(SIZE, 0, SIZE, 0);
    }


    private SKColor BackgroundColor => Value switch {
        0 => CatppuccinMocha.Surface0,
        2 => CatppuccinMocha.Yellow,
        4 => CatppuccinMocha.Red,
        8 => CatppuccinMocha.Green,
        16 => CatppuccinMocha.Blue,
        32 => CatppuccinMocha.Lavender,
        64 => CatppuccinMocha.Peach,
        128 => CatppuccinMocha.Sky,
        256 => CatppuccinMocha.Maroon,
        512 => CatppuccinMocha.Sapphire,
        1024 => CatppuccinMocha.Pink,
        2048 => CatppuccinMocha.Mauve,
        _ => CatppuccinMocha.Rosewater
    };

    private readonly SKPaint BackgroundPaint = new() {
        Color = CatppuccinMocha.Empty,
        Style = SKPaintStyle.Fill,
    };

    private static SKPaint TextPaint => new() {
        Color = CatppuccinMocha.Surface0,
        Style = SKPaintStyle.Fill,
        TextAlign = SKTextAlign.Center,
        IsAntialias = true,
        TextSize = TEXT_SIZE
    };

    public Cell(int val) {
        Value = val;
    }

    protected override void Dispose(bool disposing) {
        TextPaint.Dispose();
        BackgroundPaint.Dispose();
        base.Dispose(disposing);
    }

    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.DrawRoundRect(0, 0, SIZE, SIZE, BORDER_RADIUS, BORDER_RADIUS, BackgroundPaint);
        canvas.DrawText(Value.ToString(), 50, TEXT_HEIGHT, TextPaint);

    }

    public bool CanMerge(Cell other) {
        if (Value == 0 || other.Value == 0) {
            return true;
        }
        return Value == other.Value;
    }
}

