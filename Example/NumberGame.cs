using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using Blade.Colors;
using Blade.Extensions;
namespace Example.NumberGame;
public class Game : Blade.Screen {

    private const int BOARD_SIZE = 4;
    private const int CELL_PADDING = 10;

    public const int WIDTH = BOARD_SIZE * Cell.SIZE + (BOARD_SIZE - 1) * CELL_PADDING;
    public const int HEIGHT = BOARD_SIZE * Cell.SIZE + (BOARD_SIZE - 1) * CELL_PADDING;

    private readonly Cell[][] Cells;
    private readonly Random Rng = new();
    private (int x, int y) PreviousSpawnLocation = (-1, -1);

    private int score = 0;

    public Game() {
        Cells = new Cell[BOARD_SIZE][];
        for (int x = 0; x < BOARD_SIZE; x++) {
            Cells[x] = new Cell[BOARD_SIZE];
            for (int y = 0; y < BOARD_SIZE; y++) {
                Cells[x][y] = new Cell(0);
            }
        }
        SpawnCell();
        SpawnCell();
    }

    private static SKPaint scorePaint = new() {
        Color = Catppuccin.Text,
        IsAntialias = true,
        TextSize = 32
    };

    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.CenterScreen(WIDTH, HEIGHT);

        for (int x = 0; x < BOARD_SIZE; x++) {
            for (int y = 0; y < BOARD_SIZE; y++) {
                canvas.DrawDrawable(Cells[x][y], x * (Cell.SIZE + CELL_PADDING), y * (Cell.SIZE + CELL_PADDING));
            }
        }
        canvas.Translate(0, -10);
        canvas.DrawText($"Score: {score}", 0, 0, scorePaint);
    }

    public override void OnKeyDown(KeyboardKeyEventArgs e) {
        switch (e.Key) {
            case Keys.Up:
                MoveUp();
                AfterMove();
                break;
            case Keys.Down:
                MoveDown();
                AfterMove();
                break;
            case Keys.Left:
                MoveLeft();
                AfterMove();
                break;
            case Keys.Right:
                MoveRight();
                AfterMove();
                break;
            case Keys.Escape:
                // Blade.ScreenManager.Back<Menu>();
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
        // Blade.ScreenManager.AddScreen(new Blade.TextBox() {
        //     Title = "Game Over",
        //     BackgroundColor = ConsoleColor.Red,
        //     OnSubmit = (sender, text) => {
        //         Leaderboard.AddScore(text, score);
        //         Leaderboard.Save();
        //         Blade.ScreenManager.Back<Menu>();
        //     },
        //     OnCancel = () => Blade.ScreenManager.Back<Menu>(),
        // });
    }


    public void MoveUp() {
        bool tryAgain = false;

        for (int y = BOARD_SIZE - 1; y > 0; y--) {

            for (int x = 0; x < BOARD_SIZE; x++) {
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
        for (int y = 0; y < BOARD_SIZE; y++) {

            for (int x = 0; x < BOARD_SIZE; x++) {
                if (Cells[x][y].Value != 0) {
                    int currentY = y;
                    while (currentY < BOARD_SIZE - 1) {
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

        for (int y = 0; y < BOARD_SIZE; y++) {

            for (int x = BOARD_SIZE - 1; x > 0; x--) {
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

        for (int y = 0; y < BOARD_SIZE; y++) {

            for (int x = 0; x < BOARD_SIZE; x++) {
                if (Cells[x][y].Value != 0) {
                    int currentX = x;
                    while (currentX < BOARD_SIZE - 1) {
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
        int x = Rng.Next(0, BOARD_SIZE);
        int y = Rng.Next(0, BOARD_SIZE);
        int attempts = 0;
        // Dont spawn a cell in the same location as the previous cell
        while (x == PreviousSpawnLocation.x && y == PreviousSpawnLocation.y) {
            x = Rng.Next(0, BOARD_SIZE);
            y = Rng.Next(0, BOARD_SIZE);
            attempts++;
            // If we have tried to spawn a cell in the same location 10 times
            // then just spawn a cell in the first empty cell
            if (attempts > 10) {
                for (int i = 0; i < BOARD_SIZE; i++) {
                    for (int j = 0; j < BOARD_SIZE; j++) {
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
            x = Rng.Next(0, BOARD_SIZE);
            y = Rng.Next(0, BOARD_SIZE);
        }
        Cells[x][y].Value = 2;
    }


    public bool HasEmptyCell() {
        for (int x = 0; x < BOARD_SIZE; x++) {
            for (int y = 0; y < BOARD_SIZE; y++) {
                if (Cells[x][y].Value == 0) {
                    return true;
                }
            }
        }
        return false;
    }

}

public class Cell : Blade.GameObject {
    public const int SIZE = 100;
    private const int TEXT_HEIGHT = 60;
    private const int TEXT_SIZE = 32;
    private const int BORDER_RADIUS = 10;
    private int _value;

    public int Value {
        get => _value;
        set {
            _value = value;
            BackgroundPaint.Color = BackgroundColor;
        }
    }

    protected override SKRect OnGetBounds() {
        return new(SIZE, 0, SIZE, 0);
    }


    private SKColor BackgroundColor => Value switch {
        0 => Catppuccin.Surface0,
        2 => Catppuccin.Yellow,
        4 => Catppuccin.Red,
        8 => Catppuccin.Green,
        16 => Catppuccin.Blue,
        32 => Catppuccin.Lavender,
        64 => Catppuccin.Peach,
        128 => Catppuccin.Sky,
        256 => Catppuccin.Maroon,
        512 => Catppuccin.Sapphire,
        1024 => Catppuccin.Pink,
        2048 => Catppuccin.Mauve,
        _ => Catppuccin.Rosewater
    };

    private readonly SKPaint BackgroundPaint = new() {
        Color = Catppuccin.Empty,
        Style = SKPaintStyle.Fill,
    };

    private static SKPaint TextPaint => new() {
        Color = Catppuccin.Surface0,
        Style = SKPaintStyle.Fill,
        TextAlign = SKTextAlign.Center,
        IsAntialias = true,
        TextSize = TEXT_SIZE
    };

    public Cell(int val) {
        Value = val;
    }

    protected override void Dispose(bool disposing) {
        TextPaint.Dispose();
        BackgroundPaint.Dispose();
        base.Dispose(disposing);
    }

    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.DrawRoundRect(0, 0, SIZE, SIZE, BORDER_RADIUS, BORDER_RADIUS, BackgroundPaint);
        canvas.DrawText(Value.ToString(), 50, TEXT_HEIGHT, TextPaint);

    }

    public bool CanMerge(Cell other) {
        if (Value == 0 || other.Value == 0) {
            return true;
        }
        return Value == other.Value;
    }
}

