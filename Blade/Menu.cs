namespace Blade;


public abstract class Menu : Screen {
    const int WIDTH = 40;
    int height;
    string selected = "";
    public int Padding = 5;

    public abstract Dictionary<string, Action> Options { get; }
    public virtual string Title { get; } = "Menu";
    public virtual ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Yellow;
    public override (int, int) Offset => GetCenter(WIDTH, height);
    public override int UpdateRate => 1000 / 10;


    public Menu() {
        height = (Options.Count * 2) + 5;
        selected = Options.Keys.First();
    }


    public override void Draw() {
        base.Draw();
        Paint(0, 0, new string(' ', WIDTH), ConsoleColor.White, BackgroundColor);
        Paint(0, 1, Utils.CenterText(Title, WIDTH), ConsoleColor.Black, BackgroundColor);
        Paint(0, 2, new string(' ', WIDTH), ConsoleColor.White, BackgroundColor);
        foreach (var (key, val) in Options) {
            int index = Options.Keys.ToList().IndexOf(key);
            var bgColor = key == selected ? ConsoleColor.White : ConsoleColor.Black;
            var fgColor = key == selected ? ConsoleColor.Black : ConsoleColor.White;

            Paint(0, 3 + (index * 2), new string(' ', Padding), ConsoleColor.White, BackgroundColor);
            Paint(Padding, 3 + (index * 2), Utils.CenterText(key, WIDTH - (Padding * 2)), fgColor, bgColor);
            Paint(WIDTH - Padding, 3 + (index * 2), new string(' ', Padding), ConsoleColor.White, BackgroundColor);

            Paint(0, 4 + (index * 2), new string(' ', WIDTH), ConsoleColor.White, BackgroundColor);

        }
    }

    public override void OnKeyPress(ConsoleKeyInfo key) {
        base.OnKeyPress(key);
        var keys = Options.Keys.ToList();
        switch (key.Key) {
            case ConsoleKey.UpArrow:
                int prevI = keys.IndexOf(selected) - 1;
                if (prevI >= 0) {
                    selected = keys[prevI];
                }
                break;
            case ConsoleKey.DownArrow:
                int nextI = keys.IndexOf(selected) + 1;
                if (nextI < keys.Count) {
                    selected = keys[nextI];
                }
                break;
            case ConsoleKey.Enter:
                Options[selected].Invoke();
                break;
            case ConsoleKey.Escape:
                Options["Exit"]?.Invoke();
                Options["Cancel"]?.Invoke();
                break;
        }
    }

}