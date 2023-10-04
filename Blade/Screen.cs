namespace Blade;

public class Screen : GameObject {
    public required string Route { get; set; }
}

public delegate void GameOverDelegate(int score);

public class GameScreen : Screen {
    public required Action OnExit { get; init; }
    public required GameOverDelegate OnGameOver { get; init; }
    public required Leaderboard Leaderboard { get; init; }
}