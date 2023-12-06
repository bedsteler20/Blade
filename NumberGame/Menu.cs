namespace NumberGame;
public class Menu : Blade.Menu {
    public override string Title => "2048";
    public override ConsoleColor BackgroundColor => ConsoleColor.Red;
    public Blade.Leaderboard leaderboard = new("NumberGame");

    public override Dictionary<string, Action> Options => new() {
        ["Play"] = () => Blade.ScreenManager.AddScreen(new Game(4, leaderboard)),
        ["Leaderboard"] = () => Blade.ScreenManager.AddScreen(new Blade.LeaderboardMenu() {
            Leaderboard = leaderboard,
            Close = () => Blade.ScreenManager.Back()
        }),
        ["Exit"] = () => Blade.ScreenManager.Back(),
    };
}