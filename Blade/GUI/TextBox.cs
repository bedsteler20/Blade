using Blade.Colors;
using Blade.Extensions;
using SkiaSharp;

namespace Blade.GUI;

enum Focus {
    Text,
    Submit,
    Cancel
}

public class TextBox : Screen {
    const int PADDING = 20;
    const int TITLE_SIZE = 24;
    const int ENTRY_HEIGHT = 30;
    const int ENTRY_WIDTH = 250;
    const int ENTRY_PADDING = 4;
    const int BUTTON_WIDTH = 100;
    const int BUTTON_HEIGHT = 24;
    const int BUTTON_TEXT_SIZE = 18;
    const int BUTTON_PADDING = 10;
    const int BUTTON_MARGIN = 10;
    const int BORDER_RADIUS = 10;
    const int HEIGHT = PADDING +
                       TITLE_SIZE +
                       BUTTON_MARGIN +
                       ENTRY_PADDING +
                       ENTRY_HEIGHT +
                       ENTRY_PADDING +
                       BUTTON_MARGIN +
                       BUTTON_HEIGHT +
                       PADDING;
    const int WIDTH = PADDING +
                      ENTRY_PADDING +
                      ENTRY_WIDTH +
                      ENTRY_PADDING +
                      PADDING;

    private Focus _focus = Focus.Text;

    private Focus Focus {
        get => _focus;
        set {
            _focus = value;
            CancelButton.IsFocused = false;
            SubmitButton.IsFocused = false;

            if (value == Focus.Submit) {
                SubmitButton.IsFocused = true;
            } else if (value == Focus.Cancel) {
                CancelButton.IsFocused = true;
            }
        }
    }
    public required Action OnCancel { get; init; }
    public required Action<string> OnSubmit { get; init; }
    public required string Title { get; init; }
    public required string SubmitText {
        get => SubmitButton.Text;
        init => SubmitButton.Text = value;
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
        TextSize = 20,
        Typeface = Catppuccin.Font
    };

    private readonly SKPaint EntryBackgroundPaint = new() {
        Color = Catppuccin.Overlay1
    };

    private readonly Button SubmitButton;
    private readonly Button CancelButton;
    private string Text = "UwU OwO";

    public TextBox() {
        SubmitButton = new Button() {
            Text = null,
            OnClick = () => OnSubmit(Text),
            Width = BUTTON_WIDTH,
            TextHeight = BUTTON_TEXT_SIZE,
            Height = BUTTON_HEIGHT
        };
        CancelButton = new Button() {
            Text = null,
            OnClick = () => OnCancel(),
            Width = BUTTON_WIDTH,
            TextHeight = BUTTON_TEXT_SIZE,
            Height = BUTTON_HEIGHT,

        };
        Children.Add(CancelButton);
        Children.Add(SubmitButton);
    }

    protected override void OnDraw(SKCanvas canvas) {
        canvas.CenterScreen(WIDTH, HEIGHT);
        canvas.DrawRoundRect(0, 0, WIDTH, HEIGHT, BORDER_RADIUS, BORDER_RADIUS, BackgroundPaint);
        canvas.DrawText(Title, WIDTH / 2, PADDING + TITLE_SIZE, TextPaint);
        canvas.Translate(PADDING, PADDING + TITLE_SIZE + PADDING);
        canvas.DrawRect(0, 0, ENTRY_WIDTH, ENTRY_HEIGHT, EntryBackgroundPaint);
        canvas.DrawText(Text, ENTRY_WIDTH / 2, ENTRY_HEIGHT - ENTRY_PADDING, TextPaint);
        canvas.Translate(0, ENTRY_HEIGHT + ENTRY_PADDING + BUTTON_MARGIN);
        const int center = ((WIDTH) / 2) - ((BUTTON_WIDTH + BUTTON_PADDING + BUTTON_WIDTH) / 2) - PADDING;
        canvas.Translate(center, 0);
        canvas.DrawDrawable(SubmitButton, 0, 0);
        canvas.Translate(BUTTON_WIDTH + BUTTON_PADDING, 0);
        canvas.DrawDrawable(CancelButton, 0, 0);
    }



}