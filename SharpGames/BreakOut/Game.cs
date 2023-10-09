using Blade;
using Blade.Colors;
using Blade.Extensions;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;

namespace SharpGames.BreakOut;

public class Game : GameScreen {
    private const int PADDLE_WIDTH = 120;
    private const int PADDLE_HEIGHT = 20;
    private const int BALL_SIZE = 20;
    private const int BRICK_WIDTH = 60;
    private const int BRICK_HEIGHT = 30;
    private const int BRICKS_PER_ROW = 10;
    private const int BRICK_GAP = 8;
    private const int BRICK_ROWS = 5;
    private const int PADDING = 15;
    private const int BLANK_SPACE = 200;
    private const int HEIGHT = PADDING + PADDLE_HEIGHT + PADDING + PADDING + BLANK_SPACE +
                     (BRICK_HEIGHT * BRICK_ROWS) + ((BRICK_ROWS - 1) * BRICK_GAP) + PADDING;
    private const int WIDTH = PADDING + (BRICK_WIDTH * BRICKS_PER_ROW) +
                     ((BRICKS_PER_ROW - 1) * BRICK_GAP) + PADDING;

    private (int x, int y) PaddlePosition = (WIDTH / 2 - PADDLE_WIDTH / 2, HEIGHT - PADDLE_HEIGHT - PADDING);
    private (float x, float y) BallPosition = (WIDTH / 2 - BALL_SIZE / 2, HEIGHT / 2 - BALL_SIZE / 2);
    private (float x, float y) BallVelocity = (1, -1);
    private (int x, int y)[] Bricks = new (int x, int y)[BRICKS_PER_ROW * BRICK_ROWS];
    private int Score = 0;

    private static readonly SKPaint ScorePaint = new() {
        Color = Catppuccin.Text,
        IsAntialias = true,
        TextSize = 24,
        Typeface = Catppuccin.Font
    };

    private static readonly SKPaint BorderPaint = new() {
        Color = Catppuccin.Text,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 4
    };

    private static readonly SKPaint PaddlePaint = new() {
        Color = Catppuccin.Text,
        Style = SKPaintStyle.Fill
    };

    private static readonly SKPaint BallPaint = new() {
        Color = Catppuccin.Text,
        Style = SKPaintStyle.Fill
    };

    private readonly SKPaint BrickPaint = new() {
        Color = Catppuccin.Text,
        Style = SKPaintStyle.Fill
    };

    protected override void Setup() {
        base.Setup();
        SpawnBricks();
    }

    private void SpawnBricks() {
        for (int i = 0; i < BRICK_ROWS; i++) {
            for (int j = 0; j < BRICKS_PER_ROW; j++) {
                Bricks[i * BRICKS_PER_ROW + j] = (j, i);
            }
        }
    }

    private void SpeedUp() {
        if (BallVelocity.x > 0) {
            BallVelocity.x += 0.1f;
        } else {
            BallVelocity.x -= 0.1f;
        }
        if (BallVelocity.y > 0) {
            BallVelocity.y += 0.1f;
        } else {
            BallVelocity.y -= 0.1f;
        }
    }


    public override void Update() {
        base.Update();
        // Check if we need to reset when all bricks are gone
        if (Bricks.All(brick => brick == (-1, -1))) {
            SpawnBricks();
            BallPosition = (WIDTH / 2 - BALL_SIZE / 2, HEIGHT / 2 - BALL_SIZE / 2);
            BallVelocity = (1, -1);
            PaddlePosition = (WIDTH / 2 - PADDLE_WIDTH / 2, HEIGHT - PADDLE_HEIGHT - PADDING);
            Score = 0;
        }

        // Check for left/right wall collision
        if (BallPosition.x <= 1 || BallPosition.x >= WIDTH - BALL_SIZE - 1) {
            BallVelocity.x *= -1;
        }

        // Check for top wall collision
        if (BallPosition.y <= 1) {
            BallVelocity.y *= -1;
        }

        // Check for game over
        if (BallPosition.y >= HEIGHT - BALL_SIZE - 1) {
            OnGameOver(Score);
            return;
        }

        // Check for paddle collision
        if (BallPosition.x >= PaddlePosition.x && BallPosition.x <= PaddlePosition.x + PADDLE_WIDTH - 1 &&
            BallPosition.y >= PaddlePosition.y && BallPosition.y <= PaddlePosition.y + PADDLE_HEIGHT + PADDING) {
            BallVelocity.y *= -1;
        }

        // Check if the ball is colliding with a brick
        foreach ((int x, int y) in Bricks) {
            if (BallPosition.x >= PADDING + x * (BRICK_WIDTH + BRICK_GAP) && BallPosition.x <= PADDING + x * (BRICK_WIDTH + BRICK_GAP) + BRICK_WIDTH - 1 &&
                BallPosition.y >= PADDING + y * (BRICK_HEIGHT + BRICK_GAP) && BallPosition.y <= PADDING + y * (BRICK_HEIGHT + BRICK_GAP) + BRICK_HEIGHT - 1) {
                Bricks[y * BRICKS_PER_ROW + x] = (-1, -1);
                BallVelocity.y *= -1;
                Score++;
                SpeedUp();
                break;
            }
        }


        BallPosition.x += BallVelocity.x;
        BallPosition.y += BallVelocity.y;
    }

    private void InputRight(bool fast) {
        if (PaddlePosition.x < WIDTH - PADDLE_WIDTH + 1) {
            if (fast && PaddlePosition.x < WIDTH - PADDLE_WIDTH + 2) {
                PaddlePosition.x += 10;
            } else {
                PaddlePosition.x += 5;
            }
        }
    }

    private void InputLeft(bool fast) {
        if (PaddlePosition.x > 1) {
            if (fast && PaddlePosition.x > 2) {
                PaddlePosition.x -= 10;
            } else {
                PaddlePosition.x -= 5;
            }
        }
    }

    public override void OnKeyDown(KeyboardKeyEventArgs e) {
        base.OnKeyDown(e);
        switch (e.Key) {
            case Keys.Left:
                InputLeft(e.Shift);
                break;
            case Keys.Right:
                InputRight(e.Shift);
                break;
            case Keys.Escape:
                OnExit();
                break;
        }
    }

    public override void OnJoystickButtonDown(XInputMapping button) {
        base.OnJoystickButtonDown(button);
        switch (button) {
            case XInputMapping.LeftDPad:
                InputLeft(false);
                break;
            case XInputMapping.RightDPad:
                InputRight(false);
                break;
            case XInputMapping.LeftBumper:
                InputLeft(true);
                break;
            case XInputMapping.RightBumper:
                InputRight(true);
                break;
            case XInputMapping.Start:
                OnExit();
                break;
        }
    }


    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.CenterScreen(WIDTH, HEIGHT);
        canvas.DrawRect(0, 0, WIDTH, HEIGHT, BorderPaint);
        foreach ((int x, int y) in Bricks) {
            if (x == -1 && y == -1) {
                continue;
            }
            BrickPaint.Color = y switch {
                0 => Catppuccin.Red,
                1 => Catppuccin.Yellow,
                2 => Catppuccin.Green,
                3 => Catppuccin.Blue,
                4 => Catppuccin.Lavender,
                _ => Catppuccin.Text
            };
            canvas.DrawRect(PADDING + x * (BRICK_WIDTH + BRICK_GAP), PADDING + y * (BRICK_HEIGHT + BRICK_GAP), BRICK_WIDTH, BRICK_HEIGHT, BrickPaint);
        }
        canvas.DrawCircle(BallPosition.x + BALL_SIZE / 2, BallPosition.y + BALL_SIZE / 2, BALL_SIZE / 2, BallPaint);
        canvas.DrawRect(PaddlePosition.x, PaddlePosition.y, PADDLE_WIDTH, PADDLE_HEIGHT, PaddlePaint);
        canvas.Translate(0, -10);
        canvas.DrawText($"Score: {Score}", 0, 0, ScorePaint);

    }

}