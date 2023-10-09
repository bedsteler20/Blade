using Blade.Colors;
using Blade.Extensions;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;

namespace Blade.GUI;

public class ConfirmDialog : Screen {
    private const int BUTTON_WIDTH = 100;
    private const int BUTTON_HEIGHT = 30;
    private const int BUTTON_MARGIN = 12;
    private const int BOARDER_RADIUS = 8;
    private const int TEXT_SIZE = 24;
    private const int PADDING = 12;
    private const int WIDTH = PADDING + BUTTON_WIDTH + BUTTON_MARGIN + BUTTON_WIDTH + PADDING;
    private const int HEIGHT = PADDING + BUTTON_HEIGHT + BUTTON_MARGIN + TEXT_SIZE + PADDING;

    public Action CancelAction { get; init; } = () => { };
    public required Action ConfirmAction { get; init; }
    public required string Message { get; init; }
    public required string ConfirmText {
        get => ConfirmButton.Text;
        init => ConfirmButton.Text = value;
    }
    public required string CancelText {
        get => CancelButton.Text;
        init => CancelButton.Text = value;
    }
    public required SKColor BackgroundColor {
        get => BackgroundPaint.Color;
        init => BackgroundPaint.Color = value;
    }
    public required SKColor TextColor {
        get => TextPaint.Color;
        init => TextPaint.Color = value;
    }
    private readonly SKPaint BackgroundPaint = new() {
        Style = SKPaintStyle.Fill
    };
    private readonly SKPaint TextPaint = new() {
        IsAntialias = true,
        TextAlign = SKTextAlign.Center,
        TextSize = TEXT_SIZE,
        Typeface = Catppuccin.Font
    };
    private readonly Button ConfirmButton;
    private readonly Button CancelButton;

    public ConfirmDialog() {
        ConfirmButton = new Button {
            Text = null,
            OnClick = () => ConfirmAction(),
            Height = BUTTON_HEIGHT,
            Width = BUTTON_WIDTH,
            IsFocused = true
        };
        CancelButton = new Button {
            Text = null,
            OnClick = () => CancelAction(),
            Height = BUTTON_HEIGHT,
            Width = BUTTON_WIDTH,
            IsFocused = false
        };
        Children.Add(ConfirmButton);
        Children.Add(CancelButton);
    }

    public override void OnKeyDown(KeyboardKeyEventArgs e) {
        base.OnKeyDown(e);
        if (e.Key == Keys.Right || e.Key == Keys.Left) {
            ConfirmButton.IsFocused = !ConfirmButton.IsFocused;
            CancelButton.IsFocused = !CancelButton.IsFocused;
        } else if (e.Key == Keys.Enter) {
            if (ConfirmButton.IsFocused) {
                ConfirmAction();
            } else {
                CancelAction();
            }
        } else if (e.Key == Keys.Escape) {
            CancelAction();
        }
    }

    public override void OnJoystickButtonPressed(XInputMapping button) {
        base.OnJoystickButtonPressed(button);
        if (button == XInputMapping.RightDPad || button == XInputMapping.LeftDPad) {
            ConfirmButton.IsFocused = !ConfirmButton.IsFocused;
            CancelButton.IsFocused = !CancelButton.IsFocused;
        } else if (button == XInputMapping.A) {
            if (ConfirmButton.IsFocused) {
                ConfirmAction();
            } else {
                CancelAction();
            }
        } else if (button == XInputMapping.B) {
            CancelAction();
        }
    }

    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.CenterScreen(WIDTH, HEIGHT);
        canvas.DrawRoundRect(0, 0, WIDTH, HEIGHT, BOARDER_RADIUS, BOARDER_RADIUS, BackgroundPaint);
        canvas.DrawText(Message, WIDTH / 2, PADDING + TEXT_SIZE, TextPaint);
        canvas.Translate(0, BUTTON_MARGIN + TEXT_SIZE);
        canvas.DrawDrawable(ConfirmButton, PADDING, PADDING);
        canvas.DrawDrawable(CancelButton, PADDING + BUTTON_WIDTH + BUTTON_MARGIN, PADDING);
    }

    protected override void Dispose(bool disposing) {
        BackgroundPaint.Dispose();
        TextPaint.Dispose();
        CancelButton.Dispose();
        ConfirmButton.Dispose();
        base.Dispose(disposing);
    }
}