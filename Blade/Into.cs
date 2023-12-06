namespace Blade;

public class Intro : Screen {

    public required Action OnDone { get; set; }

    public int Width = 43;
    public int Height = 5;

    public override (int, int) Offset => GetCenter(Width, Height);

    public static bool Skip {
        get {
            var e = Environment.GetEnvironmentVariable("BLADE_SKIP_INTRO");
            return e != null && (e == "1" || e.ToLower() == "true");
        }
    }


    public override void Draw() {
        base.Draw();
        if (!Skip) {
            Paint(0, 0, "    ▟████▖  ▟█▛     ▗▆███▖  ▟████▖  ▟█████▛");
            Paint(0, 1, "   ▟█▛ ▟█▛ ▟█▛     ▟█▛ ▟█▛ ▟█▛ ▐█▛ ▟█▛     ");
            Paint(0, 2, "  ▟████▛▘ ▟█▛     ▟█████▛ ▟█▛ ▟█▛ ▟███▛    ");
            Paint(0, 3, " ▟█▛ ▗█▘ ▟█▛     ▟█▛ ▟█▛ ▟█▛ ▄█▛ ▟█▛       ");
            Paint(0, 4, "▟████▛  ▟█████▛ ▟█▛ ▟█▛ ▟████▛▘ ▟█████▛    ");
            Thread.Sleep(1000);
        }
        OnDone();
    }


}