using System.Numerics;
using Blade;
using Blade.Colors;
using Blade.Extensions;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;

namespace SharpGames.Snake;

enum Direction {
    Up,
    Down,
    Left,
    Right
}

public class Game : GameScreen {
    private const int CELL_SIZE = 24;
    private const int CELLS_Y = 15;
    private const int CELLS_X = 18;
    private const int PADDING = 10;
    private const int HEIGHT = PADDING + CELL_SIZE * CELLS_Y + PADDING;
    private const int WIDTH = PADDING + CELL_SIZE * CELLS_X + PADDING;

    private Random Rng = Utils.CreateRadom();
    private Direction Direction = Direction.Right;
    private readonly Blade.Timeout Timeout;
    private int Score = 0;

    private (int x, int y)[] Snake = new[] { (9, 9), (9, 8), (9, 7), (9, 6) };
    private ref (int x, int y) SnakeHead => ref Snake[0];

    private (int x, int y) Food = (0, 0);

    public Game() {
        Timeout = new Blade.Timeout(Move, 500);
    }

    protected override void Setup() {
        base.Setup();
        SpawnFood();
        Children.Add(Timeout);
    }

    private void SpawnFood() {
        int x = Rng.Next(0, CELLS_X);
        int y = Rng.Next(0, CELLS_Y);
        if (Snake.Contains((x, y))) {
            SpawnFood();
        } else {
            Food = (x, y);
        }
    }

    public override void OnKeyDown(KeyboardKeyEventArgs e) {
        base.OnKeyDown(e);
        switch (e.Key) {
            case Keys.Up:
                Direction = Direction.Up;
                break;
            case Keys.Down:
                Direction = Direction.Down;
                break;
            case Keys.Left:
                Direction = Direction.Left;
                break;
            case Keys.Right:
                Direction = Direction.Right;
                break;
            case Keys.Escape:
                OnExit();
                break;
            default:
                break;
        }
    }

    public override void OnJoystickButtonPressed(XInputMapping button) {
        base.OnJoystickButtonPressed(button);
        switch (button) {
            case XInputMapping.UpDPad:
                Direction = Direction.Up;
                break;
            case XInputMapping.DownDPad:
                Direction = Direction.Down;
                break;
            case XInputMapping.LeftDPad:
                Direction = Direction.Left;
                break;
            case XInputMapping.RightDPad:
                Direction = Direction.Right;
                break;
            case XInputMapping.B:
                OnExit();
                break;
            default:
                break;
        }
    }

    private void Move() {
        (int x, int y) next = Direction switch {
            Direction.Up => (SnakeHead.x, SnakeHead.y - 1),
            Direction.Down => (SnakeHead.x, SnakeHead.y + 1),
            Direction.Left => (SnakeHead.x - 1, SnakeHead.y),
            Direction.Right => (SnakeHead.x + 1, SnakeHead.y),
            _ => (SnakeHead.x, SnakeHead.y),
        };
        if (next.x < 0 || next.x >= CELLS_X || next.y < 0 || next.y >= CELLS_Y) {
            OnGameOver(Score);
            return;
        }
        if (Snake.Contains(next)) {
            OnGameOver(Score);
            return;
        }
        var newSnake = Snake.Prepend(next).ToArray();

        if (next == Food) {
            Snake = newSnake;
            Score++;
            if (Timeout.Milliseconds > 100) {
                Timeout.Milliseconds -= 10;
            }
            SpawnFood();
        } else {
            Snake = newSnake[..Snake.Length];
        }
    }

    private readonly SKPaint BroaderPaint = new() {
        Color = Catppuccin.Text,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 4
    };

    private readonly SKPaint FoodPaint = new() {
        Color = Catppuccin.Red,
        Style = SKPaintStyle.Fill
    };

    private readonly SKPaint SnakePaint = new() {
        Color = Catppuccin.Green,
        Style = SKPaintStyle.Fill
    };

    private static readonly SKPaint ScorePaint = new() {
        Color = Catppuccin.Text,
        IsAntialias = true,
        TextSize = 24,
        Typeface = Catppuccin.Font
    };

    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.CenterScreen(WIDTH, HEIGHT);
        canvas.DrawRect(0, 0, WIDTH, HEIGHT, BroaderPaint);
        foreach ((int x, int y) in Snake) {
            canvas.DrawRect(PADDING + x * CELL_SIZE, PADDING + y * CELL_SIZE, CELL_SIZE, CELL_SIZE, SnakePaint);
        }
        canvas.DrawRect(PADDING + Food.x * CELL_SIZE, PADDING + Food.y * CELL_SIZE, CELL_SIZE, CELL_SIZE, FoodPaint);

        canvas.Translate(0, -10);
        canvas.DrawText($"Score: {Score}", 0, 0, ScorePaint);
    }
}