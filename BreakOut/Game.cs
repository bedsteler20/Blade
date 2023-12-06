

namespace BreakOut;

public class Game : Blade.Screen {
    const int PADDLE_WIDTH = 8;
    const int BALL_SIZE = 1;
    const int BRICK_WIDTH = 6;
    const int BRICK_HEIGHT = 1;
    const int BRICK_GAP = 1;
    const int BRICKS_PER_ROW = 10;
    const int BRICK_ROWS = 5;
    const int HEIGHT = 20;
    const int WIDTH = (BRICK_WIDTH + BRICK_GAP) * BRICKS_PER_ROW - 1;

    public override (int x, int y) Offset => GetCenter(WIDTH + 2, HEIGHT + 3);
    public override int UpdateRate => 1000 / 10;

    private (int x, int y) paddlePosition = ((WIDTH / 2) - (PADDLE_WIDTH / 2), HEIGHT);
    private (int x, int y) ballPosition = ((WIDTH / 2) - (BALL_SIZE / 2), HEIGHT - 2);
    private (int x, int y) ballVelocity = (1, -1);
    private (int x, int y)[] brickPositions = new (int x, int y)[BRICKS_PER_ROW * BRICK_ROWS];
    private Blade.Leaderboard leaderboard;
    private int score = 0;
    public Game(Blade.Leaderboard leaderboard) {
        this.leaderboard = leaderboard;
        SpawnBricks();
    }


    public override void Draw() {
        base.Draw();
        Paint(0, 0, $"Score: {score}");
        DrawBorder(WIDTH, HEIGHT, 0, 1);

        //  Draw the paddle
        for (int i = 0; i < PADDLE_WIDTH; i++) {
            Paint(paddlePosition.x + i, paddlePosition.y, "▀");
        }

        // Draw the bricks
        int c = 0;
        int py = 0;
        foreach ((int x, int y) in brickPositions) {
            if (x == -1 || y == -1) continue;
            if (py != y) {
                c++;
            }
            py = y;
            Console.ForegroundColor = c switch {
                0 => ConsoleColor.Red,
                1 => ConsoleColor.Red,
                2 => ConsoleColor.Yellow,
                3 => ConsoleColor.Green,
                4 => ConsoleColor.Cyan,
                5 => ConsoleColor.Blue,
                6 => ConsoleColor.Magenta,
                _ => ConsoleColor.Red
            };

            PaintRect(x, y, x + BRICK_WIDTH - 1, y + BRICK_HEIGHT - 1, "█");
            Console.ResetColor();

        }

        Paint(ballPosition.x, ballPosition.y, "◍");
    }

    private void SpawnBricks() {
        for (int i = 0; i < BRICKS_PER_ROW * BRICK_ROWS; i++) {
            int x = i % BRICKS_PER_ROW * (BRICK_WIDTH + BRICK_GAP) + 1;
            int y = i / BRICKS_PER_ROW * (BRICK_HEIGHT + BRICK_GAP) + 3;
            brickPositions[i] = (x, y);
        }
    }

    public override void Update() {
        base.Update();
        // Check if we need to reset
        if (brickPositions.All(d => d.x == -1 || d.y == -1)) {
            SpawnBricks();
            ballPosition = ((WIDTH / 2) - (BALL_SIZE / 2), HEIGHT - 3);
            ballVelocity = (1, -1);
        }
        // Check for left/right wall collision
        if (ballPosition.x == 1 || ballPosition.x == WIDTH - BALL_SIZE + 1) {
            ballVelocity.x *= -1;
        }

        // Check for top wall collision
        if (ballPosition.y <= 1) {
            ballVelocity.y *= -1;
        }

        // Check for game over
        if (ballPosition.y == HEIGHT) {
            GameOver();
            return;
        }

        // Check for paddle collision
        if (ballPosition.y == paddlePosition.y - 1) {
            if (ballPosition.x >= paddlePosition.x && ballPosition.x <= paddlePosition.x + PADDLE_WIDTH - 1) {
                ballVelocity.y *= -1;
            }
        }

        // Check for brick collision
        for (int i = 0; i < BRICKS_PER_ROW * BRICK_ROWS; i++) {
            int x = brickPositions[i].x;
            int y = brickPositions[i].y;
            if (ballPosition.x >= x && ballPosition.x <= x + BRICK_WIDTH - 1 && ballPosition.y == y - 1) {
                ballVelocity.y *= -1;
                brickPositions[i] = (-1, -1);
                score++;
            }
        }


        ballPosition.x += ballVelocity.x;
        ballPosition.y += ballVelocity.y;
    }

    private void GameOver() {
        Blade.ScreenManager.AddScreen(new Blade.GameOver() {
            OnGameOver = () => Blade.ScreenManager.AddScreen(new Blade.TextBox() {
                Title = "Game Over",
                BackgroundColor = ConsoleColor.Cyan,
                OnSubmit = (sender, text) => {
                    leaderboard.AddScore(text, score);
                    leaderboard.Save();
                    Blade.ScreenManager.Back<Menu>();
                },
                OnCancel = Blade.ScreenManager.Back<Menu>,
            })
        });
    }

    public override void OnKeyPress(ConsoleKeyInfo key) {
        base.OnKeyPress(key);
        switch (key.Key) {
            case ConsoleKey.LeftArrow:
                if (paddlePosition.x > 1) {
                    if (key.Modifiers == ConsoleModifiers.Shift && paddlePosition.x > 2) {
                        paddlePosition.x -= 2;
                    } else {
                        paddlePosition.x--;

                    }
                }
                break;
            case ConsoleKey.RightArrow:
                if (paddlePosition.x < WIDTH - PADDLE_WIDTH + 1) {
                    if (key.Modifiers == ConsoleModifiers.Shift && paddlePosition.x < WIDTH - PADDLE_WIDTH + 2) {
                        paddlePosition.x += 2;
                    } else {
                        paddlePosition.x++;

                    }
                }
                break;
            case ConsoleKey.Escape:
                Blade.ScreenManager.Back<Menu>();
                break;
        }
    }

}


public class Menu : Blade.Menu {
    public override string Title => "BreakOut";
    public override ConsoleColor BackgroundColor => ConsoleColor.Cyan;
    private Blade.Leaderboard leaderboard = new("BreakOut");
    public override Dictionary<string, Action> Options => new() {
        ["Play"] = () => Blade.ScreenManager.AddScreen(new Game(leaderboard)),
        ["Leaderboard"] = () => Blade.ScreenManager.AddScreen(new Blade.LeaderboardMenu() {
            Leaderboard = leaderboard,
            bgColor = ConsoleColor.Cyan,
            Close = Blade.ScreenManager.Back<Menu>
        }),
        ["Exit"] = Blade.ScreenManager.Back
    };
};
