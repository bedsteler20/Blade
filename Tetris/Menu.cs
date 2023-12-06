namespace Tetris;

public class Menu : Blade.Menu
{
    private readonly Blade.Leaderboard leaderboard = new("Tetris");
    public override ConsoleColor BackgroundColor => ConsoleColor.Magenta;
    public override string Title => "Tetris";
    public override Dictionary<string, Action> Options => new()
    {
        ["Play"] = () => Blade.ScreenManager.AddScreen(new Game(leaderboard)),
        ["Leaderboard"] = () => Blade.ScreenManager.AddScreen(new Blade.LeaderboardMenu()
        {
            Leaderboard = leaderboard,
            bgColor = ConsoleColor.Magenta,
            Close = Blade.ScreenManager.Back<Menu>
        }),
        ["Exit"] = () => Blade.ScreenManager.Back()
    };
}