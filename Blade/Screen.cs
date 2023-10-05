namespace Blade;

public class Screen : GameObject {
    public string Route { get; set; }
}

public delegate void GameOverDelegate(int score);

public class GameScreen : Screen {
    public Action OnExit { get; init; }
    public GameOverDelegate OnGameOver { get; init; }
    public Leaderboard Leaderboard { get; init; }
    public GameScreen() { }
}