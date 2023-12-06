namespace Blade;

class GameOver : Screen {
    const int HEIGHT = 11, WIDTH = 38;

    public override (int x, int y) Offset => GetCenter(WIDTH, HEIGHT);

    public required virtual Action OnGameOver { get; set; }
    public override int UpdateRate => 1000;

    bool submitted = false;

    public override void Draw() {
        base.Draw();
        Paint(0, 00, "   ▗▄████▛ ▗▄███▙  ▟████████▙  ▟█████▛", ConsoleColor.Red);
        Paint(0, 01, "  ▟█▛▘    ▟█▛▘▟█▛ ▟█▛▘▟█▛▘▟█▛ ▟█▛     ", ConsoleColor.Red);
        Paint(0, 02, " ▟█▛ ▟█▛▘▟█████▛ ▟█▛ ▟█▛ ▟█▛ ▟███▛    ", ConsoleColor.Red);
        Paint(0, 03, "▟█▛ ▟█▛ ▟█▛ ▟█▛ ▟█▛ ▟█▛ ▟█▛ ▟█▛       ", ConsoleColor.Red);
        Paint(0, 04, "▜███▛▘ ▟█▛ ▟█▛ ▟█▛ ▟█▛ ▟█▛ ▟█████▛    ", ConsoleColor.Red);
        Paint(0, 05, "                                      ", ConsoleColor.Red);
        Paint(0, 06, "   ▗▄███▙  ▟█▛ ▟█▛ ▟█████▛ ▟████▙     ", ConsoleColor.Red);
        Paint(0, 07, "  ▟█▛ ▟█▛ ▟█▛ ▟█▛ ▟█▛     ▟█▛ ▟█▛     ", ConsoleColor.Red);
        Paint(0, 08, " ▟█▛ ▟█▛ ▟█▛ ▟█▛ ▟███▛   ▟█████▘      ", ConsoleColor.Red);
        Paint(0, 09, "▟█▛ ▟█▛  ▜█▙▟▛▘ ▟█▛     ▟█▛▘▟█▛       ", ConsoleColor.Red);
        Paint(0, 10, "▜███▛▘    ▜▛▘  ▟█████▛ ▟█▛ ▟█▛        ", ConsoleColor.Red);
    }

    public override void Update() {
        base.Update();
        if (submitted) {
            OnGameOver();

        }
        submitted = true;
    }
}