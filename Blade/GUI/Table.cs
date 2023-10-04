using Blade.Colors;
using Blade.Extensions;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;

namespace Blade.GUI;

public class Table : Screen {
    private const int ROWS = 10;
    private const int COLUMNS = 2;
    private const int PADDING = 15;
    private const int ROW_HEIGHT = 24;
    private const int COLUMN_WIDTH = 200;
    private const int CELL_PADDING = 4;
    private const int INNER_WIDTH = COLUMNS * COLUMN_WIDTH;
    private const int INNER_HEIGHT = ROWS * ROW_HEIGHT + (ROWS - 1) * CELL_PADDING;
    private const int WIDTH = 2 * (PADDING + CELL_PADDING) + INNER_WIDTH;
    private const int HEIGHT = PADDING + HEADER_TEXT_SIZE + PADDING + INNER_HEIGHT + PADDING + BUTTON_HEIGHT + PADDING;
    private const int BUTTON_WIDTH = 100;
    private const int BUTTON_HEIGHT = 30;
    private const int HEADER_TEXT_SIZE = 30;
    private const int TEXT_SIZE = 20;
    private const int BORDER_RADIUS = 20;
    public required Dictionary<string, string> Data { get; init; }
    public required string Title { get; init; }
    public required SKColor BackgroundColor {
        get => BackgroundPaint.Color;
        init => BackgroundPaint.Color = value;
    }
    public required SKColor BodyTextColor {
        get => TextPaintL.Color;
        init {
            TextPaintL.Color = value;
            TextPaintR.Color = value;
        }
    }
    public required SKColor HeaderTextColor {
        get => HeaderPaint.Color;
        init => HeaderPaint.Color = value;
    }



    private readonly SKPaint InnerPaint = new() {
        Color = CatppuccinMocha.Surface1,
        Style = SKPaintStyle.Fill,
    };

    private readonly SKPaint BackgroundPaint = new() {
        Style = SKPaintStyle.Fill,
    };

    private readonly SKPaint TextPaintL = new() {
        TextAlign = SKTextAlign.Left,
        IsAntialias = true,
        TextSize = TEXT_SIZE,
    };

    private readonly SKPaint TextPaintR = new() {
        TextAlign = SKTextAlign.Right,
        IsAntialias = true,
        TextSize = TEXT_SIZE,
    };

    private readonly SKPaint HeaderPaint = new() {
        TextAlign = SKTextAlign.Center,
        IsAntialias = true,
        TextSize = HEADER_TEXT_SIZE,
    };

    private readonly Button BackButton = new() {
        Text = "Back",
        OnClick = ScreenManager.Back,
        Height = BUTTON_HEIGHT,
        Width = BUTTON_WIDTH,
        IsFocused = true,
    };


    public override void OnKeyDown(KeyboardKeyEventArgs e) {
        base.OnKeyDown(e);
        if (e.Key == Keys.Escape || e.Key == Keys.Enter) {
            ScreenManager.Back();
        }
    }

    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.CenterScreen(WIDTH, HEIGHT);
        canvas.DrawRoundRect(0, 0, WIDTH, HEIGHT, BORDER_RADIUS, BORDER_RADIUS, BackgroundPaint);
        canvas.DrawRect(PADDING, PADDING + HEADER_TEXT_SIZE + PADDING, INNER_WIDTH, INNER_HEIGHT, InnerPaint);
        int y = PADDING + HEADER_TEXT_SIZE + PADDING, c = 1;
        foreach (var (key, value) in Data) {
            if (c > ROWS) break;
            canvas.DrawText(key, PADDING + CELL_PADDING, y + ROW_HEIGHT / 2 + TextPaintL.TextSize / 2, TextPaintL);
            canvas.DrawText(value, PADDING + COLUMN_WIDTH + COLUMN_WIDTH - CELL_PADDING,
                            y + ROW_HEIGHT / 2 + TextPaintL.TextSize / 2, TextPaintR);
            y += ROW_HEIGHT + CELL_PADDING;
            c++;
        }
        canvas.DrawDrawable(BackButton, WIDTH / 2 - BUTTON_WIDTH / 2, HEIGHT - PADDING - BUTTON_HEIGHT);
        canvas.DrawText(Title, WIDTH / 2, PADDING + HEADER_TEXT_SIZE, HeaderPaint);
    }

    protected override void Dispose(bool disposing) {
        InnerPaint.Dispose();
        BackgroundPaint.Dispose();
        TextPaintL.Dispose();
        TextPaintR.Dispose();
        HeaderPaint.Dispose();
        BackButton.Dispose();
        base.Dispose(disposing);
    }
}