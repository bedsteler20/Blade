namespace Blade;

public delegate void TextBoxSubmitHandler(TextBox sender, string text);

enum Focus { Cancel, Submit, Text }

public class TextBox : Screen {
    public override bool ShowCursor => focus == Focus.Text;
    public override (int x, int y) Offset => GetCenter(Width, Height);
    public override int UpdateRate => 1000 / 10;

    public string Title { get; set; } = "";
    public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Yellow;
    public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Black;
    public ConsoleColor TextColor { get; set; } = ConsoleColor.White;

    public required TextBoxSubmitHandler OnSubmit { get; set; }
    public required Action OnCancel { get; set; }

    const int PADDING = 4;
    const int NAME_WIDTH = 25;
    const int BUTTON_SIZE = 8;
    const int BUTTON_PADDING = 2;
    public string Text { get; set; } = "";
    public int Width => NAME_WIDTH + PADDING * 2;
    public int Height => 1 + PADDING * 2;

    private Focus focus = Focus.Text;


    public override void Draw() {
        base.Draw();
        Paint(0, 0, '▗' + new string('▄', Width - 2) + '▖', BackgroundColor);
        Paint(0, 1, "▐", BackgroundColor);
        Paint(1, 1, Utils.CenterText(Title, Width - 2), ConsoleColor.Black, BackgroundColor);
        Paint(Width - 1, 1, "▌", BackgroundColor);

        Paint(0, 2, "▐", BackgroundColor);
        Paint(1, 2, "██▛" + new string('▀', NAME_WIDTH) + "▜██", BackgroundColor, ForegroundColor);
        Paint(Width - 1, 2, "▌", BackgroundColor);

        Paint(0, 3, "▐", BackgroundColor);
        Paint(1, 3, "██▌" + new string(' ', NAME_WIDTH) + "▐██", BackgroundColor, ForegroundColor);
        Paint(Width - 1, 3, "▌", BackgroundColor);
        Paint(4, 3, Text, TextColor, ForegroundColor);

        Paint(0, 4, "▐", BackgroundColor);
        Paint(1, 4, "██▙" + new string('▄', NAME_WIDTH) + "▟██", BackgroundColor, ForegroundColor);
        Paint(Width - 1, 4, "▌", BackgroundColor);


        Paint(0, 5, '▐' + new string('█', Width - 2) + '▌', BackgroundColor);

        Paint((Width / 2) - BUTTON_SIZE - (BUTTON_PADDING / 2), 5,
              Utils.CenterText("Submit", BUTTON_SIZE),
              focus == Focus.Submit ? ForegroundColor : TextColor,
              focus == Focus.Submit ? TextColor : ForegroundColor);

        Paint((Width / 2) + (BUTTON_PADDING / 2), 5,
              Utils.CenterText("Cancel", BUTTON_SIZE),
              focus == Focus.Cancel ? ForegroundColor : TextColor,
              focus == Focus.Cancel ? TextColor : ForegroundColor);

        Paint(0, 6, '▝' + new string('▀', Width - 2) + '▘', BackgroundColor);
        MoveCursor(4 + Text.Length, 3);
    }

    public override void OnKeyPress(ConsoleKeyInfo key) {
        base.OnKeyPress(key);
        switch (key.Key) {
            case ConsoleKey.Enter:
                if (focus == Focus.Submit) {
                    OnSubmit(this, Text);
                } else if (focus == Focus.Cancel) {
                    OnCancel();
                } else {
                    focus = Focus.Submit;
                }
                break;
            case ConsoleKey.UpArrow:
                if (focus == Focus.Cancel || focus == Focus.Submit) {
                    focus = Focus.Text;
                }
                break;
            case ConsoleKey.DownArrow:
                if (focus == Focus.Text) {
                    focus = Focus.Submit;
                }
                break;
            case ConsoleKey.RightArrow:
                if (focus == Focus.Submit) {
                    focus = Focus.Cancel;
                }
                break;
            case ConsoleKey.LeftArrow:
                if (focus == Focus.Cancel) {
                    focus = Focus.Submit;
                }
                break;
            case ConsoleKey.Backspace:
                if (Text.Length > 0 && focus == Focus.Text) {
                    Text = Text[..^1];
                }
                break;
            default:
                if (key.KeyChar >= 32 && key.KeyChar <= 126) {
                    if (Text.Length < NAME_WIDTH - 1 && focus == Focus.Text) {
                        Text += key.KeyChar;
                    }
                }
                break;
        }
    }
}