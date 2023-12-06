


namespace Snake;

public class Menu : Blade.Menu {
    public readonly Blade.Leaderboard Leaderboard = new("Snake");
    public override ConsoleColor BackgroundColor => ConsoleColor.Green;
    public override string Title => "Snake";

    public override Dictionary<string, Action> Options => new() {
        ["Play"] = () => Blade.ScreenManager.AddScreen(new Game(Leaderboard)),
        ["Leaderboard"] = () => Blade.ScreenManager.AddScreen(new Blade.LeaderboardMenu() {
            bgColor = ConsoleColor.Green,
            Leaderboard = Leaderboard,
            Close = () => Blade.ScreenManager.Back()
        }),
        ["Exit"] = () => Blade.ScreenManager.Back()
    };
}
