namespace NumberGame;
public class Game : Blade.Screen {

    private readonly Cell[][] Cells;
    private readonly Random Rng = Blade.Utils.CreateRadom();
    private (int x, int y) PreviousSpawnLocation = (-1, -1);


    public override int UpdateRate => 1000 / 5;
    private readonly Grid grid;
    private int score = 0;
    private readonly int BordSize;

    private readonly Blade.Leaderboard Leaderboard;

    const int WIDTH = 50;
    const int HEIGHT = 18;

    public override (int x, int y) Offset => GetCenter(WIDTH, HEIGHT);

    public Game(int bordSize, Blade.Leaderboard leaderboard) {
        BordSize = bordSize;
        Leaderboard = leaderboard;


        Cells = new Cell[BordSize][];
        for (int x = 0; x < BordSize; x++) {
            Cells[x] = new Cell[BordSize];
            for (int y = 0; y < BordSize; y++) {
                Cells[x][y] = new Cell(0);
            }
        }
        SpawnCell();
        SpawnCell();
        grid = new(ref Cells);
    }

    public override void OnKeyPress(ConsoleKeyInfo key) {
        switch (key.Key) {
            case ConsoleKey.UpArrow:
                MoveUp();
                AfterMove();

                break;
            case ConsoleKey.DownArrow:
                MoveDown();
                AfterMove();


                break;
            case ConsoleKey.LeftArrow:
                MoveLeft();
                AfterMove();

                break;
            case ConsoleKey.RightArrow:
                MoveRight();
                AfterMove();
                break;
            case ConsoleKey.Escape:
                Blade.ScreenManager.Back<Menu>();
                break;
            default:
                break;
        }

    }

    public void AfterMove() {
        if (!HasEmptyCell()) {
            ExitGame();
        } else {
            SpawnCell();
        }
    }

    public void ExitGame() {
        Blade.ScreenManager.AddScreen(new Blade.TextBox() {
            Title = "Game Over",
            BackgroundColor = ConsoleColor.Red,
            OnSubmit = (sender, text) => {
                Leaderboard.AddScore(text, score);
                Leaderboard.Save();
                Blade.ScreenManager.Back<Menu>();
            },
            OnCancel = () => Blade.ScreenManager.Back<Menu>(),
        });
    }


    public void MoveUp() {
        bool tryAgain = false;

        for (int y = BordSize - 1; y > 0; y--) {

            for (int x = 0; x < BordSize; x++) {
                if (Cells[x][y].Value != 0) {
                    int currentY = y;
                    while (currentY > 0) {
                        if (Cells[x][currentY - 1].Value == 0) {
                            Cells[x][currentY - 1].Value = Cells[x][currentY].Value;
                            Cells[x][currentY].Value = 0;
                            tryAgain = true;

                        } else if (Cells[x][currentY - 1].Value == Cells[x][currentY].Value) {
                            Cells[x][currentY - 1].Value *= 2;
                            score += Cells[x][currentY - 1].Value;

                            Cells[x][currentY].Value = 0;
                            tryAgain = true;
                            break;
                        } else {
                            break;
                        }
                    }
                }
            }

        }
        if (tryAgain) {
            MoveUp();
        }
    }

    public void MoveDown() {
        bool tryAgain = false;
        for (int y = 0; y < BordSize; y++) {

            for (int x = 0; x < BordSize; x++) {
                if (Cells[x][y].Value != 0) {
                    int currentY = y;
                    while (currentY < BordSize - 1) {
                        if (Cells[x][currentY + 1].Value == 0) {
                            Cells[x][currentY + 1].Value = Cells[x][currentY].Value;
                            Cells[x][currentY].Value = 0;
                            tryAgain = true;

                        } else if (Cells[x][currentY + 1].Value == Cells[x][currentY].Value) {
                            Cells[x][currentY + 1].Value *= 2;
                            score += Cells[x][currentY].Value;

                            Cells[x][currentY].Value = 0;
                            tryAgain = true;
                            break;
                        } else {
                            break;
                        }
                    }
                }
            }

        }
        if (tryAgain) {
            MoveDown();
        }
    }
    public void MoveLeft() {
        bool tryAgain = false;

        for (int y = 0; y < BordSize; y++) {

            for (int x = BordSize - 1; x > 0; x--) {
                if (Cells[x][y].Value != 0) {
                    int currentX = x;
                    while (currentX > 0) {
                        if (Cells[currentX - 1][y].Value == 0) {
                            Cells[currentX - 1][y].Value = Cells[currentX][y].Value;

                            Cells[currentX][y].Value = 0;
                            tryAgain = true;

                        } else if (Cells[currentX - 1][y].Value == Cells[currentX][y].Value) {
                            Cells[currentX - 1][y].Value *= 2;
                            score += Cells[currentX - 1][y].Value;

                            Cells[currentX][y].Value = 0;
                            tryAgain = true;
                            break;
                        } else {
                            break;
                        }
                    }
                }
            }

        }
        if (tryAgain) {
            MoveLeft();
        }
    }
    public void MoveRight() {
        bool tryAgain = false;

        for (int y = 0; y < BordSize; y++) {

            for (int x = 0; x < BordSize; x++) {
                if (Cells[x][y].Value != 0) {
                    int currentX = x;
                    while (currentX < BordSize - 1) {
                        if (Cells[currentX + 1][y].Value == 0) {
                            Cells[currentX + 1][y].Value = Cells[currentX][y].Value;
                            Cells[currentX][y].Value = 0;
                            tryAgain = true;

                        } else if (Cells[currentX + 1][y].Value == Cells[currentX][y].Value) {
                            Cells[currentX + 1][y].Value *= 2;
                            score += Cells[currentX + 1][y].Value;
                            Cells[currentX][y].Value = 0;
                            tryAgain = true;
                            break;
                        } else {
                            break;
                        }
                    }
                }
            }

        }
        if (tryAgain) {
            MoveRight();
        }
    }

    /// <summary>
    /// Spawns a new cell with a value of 2 in a random empty location on the game board.
    /// If the previous spawn location is still empty, the new cell will not be spawned in the same location.
    /// If there are no empty locations available after 10 attempts, the new cell will be spawned in the first available empty location.
    /// </summary>
    public void SpawnCell() {
        int x = Rng.Next(0, BordSize);
        int y = Rng.Next(0, BordSize);
        int attempts = 0;
        // Dont spawn a cell in the same location as the previous cell
        while (x == PreviousSpawnLocation.x && y == PreviousSpawnLocation.y) {
            x = Rng.Next(0, BordSize);
            y = Rng.Next(0, BordSize);
            attempts++;
            // If we have tried to spawn a cell in the same location 10 times
            // then just spawn a cell in the first empty cell
            if (attempts > 10) {
                for (int i = 0; i < BordSize; i++) {
                    for (int j = 0; j < BordSize; j++) {
                        if (Cells[i][j].Value == 0) {
                            x = i;
                            y = j;
                            break;
                        }
                    }
                }
                break;
            }
        }
        // Dont spawn a cell if the cell is already occupied
        while (Cells[x][y].Value != 0) {
            x = Rng.Next(0, BordSize);
            y = Rng.Next(0, BordSize);
        }
        Cells[x][y].Value = 2;
    }

    public override void Draw() {
        base.Draw();
        Paint(0, 0, $"Score: {score}");
        grid.Draw();
    }

    /// <summary>
    /// Checks if there is an empty cell in the game board.
    /// </summary>
    /// <returns>True if there is an empty cell, false otherwise.</returns>
    public bool HasEmptyCell() {
        for (int x = 0; x < BordSize; x++) {
            for (int y = 0; y < BordSize; y++) {
                if (Cells[x][y].Value == 0) {
                    return true;
                }
            }
        }
        return false;
    }

}