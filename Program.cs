class MainMenu : Blade.Menu {
    public override Dictionary<string, Action> Options => new() {
        ["2048"] = () => Blade.ScreenManager.AddScreen(new NumberGame.Menu()),
        ["Snake"] = () => Blade.ScreenManager.AddScreen(new Snake.Menu()),
        ["BreakOut"] = () => Blade.ScreenManager.AddScreen(new BreakOut.Menu()),
        ["Tetris"] = () => Blade.ScreenManager.AddScreen(new Tetris.Menu()),
        ["Exit"] = () => Environment.Exit(0)
    };
}


class Program {
    public static void Main() {
        Blade.ScreenManager.AddScreen(new MainMenu());
        Blade.ScreenManager.Run();
    }
}