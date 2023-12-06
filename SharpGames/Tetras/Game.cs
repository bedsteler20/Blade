using Blade.Colors;
using Blade.Extensions;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;

namespace SharpGames.Tetras;

public class Game : Blade.GameScreen {
    private const int BOARD_WIDTH = 18;
    private const int BOARD_HEIGHT = 28;
    private const int CELL_SIZE = 20;
    private const int WIDTH = BOARD_WIDTH * (CELL_SIZE - 1);
    private const int HEIGHT = BOARD_HEIGHT * CELL_SIZE;

    private readonly Random rng = Blade.Utils.CreateRadom();

    private PieceType[,] board = new PieceType[BOARD_WIDTH, BOARD_HEIGHT];
    private PieceType[,] piece = new PieceType[4, 4];
    private Blade.Timeout timeout;
    private (int x, int y) piecePos = (4, 2);
    private int score = 0;

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

    private readonly SKPaint BlockPaint = new() {
        Color = Catppuccin.Text,
        Style = SKPaintStyle.Fill
    };

    public Game() {
        timeout = new Blade.Timeout(1000, Drop);
    }

    protected override void Setup() {
        base.Setup();
        SpawnPiece();
        Children.Add(timeout);
    }

    private void Drop() {
        if (CanMove(0, 1)) {
            piecePos.y++;
        } else {
            for (int py = 0; py < 4; py++) {
                for (int px = 0; px < 4; px++) {
                    if (piece[px, py] == PieceType.E) {
                        continue;
                    }
                    board[piecePos.x + px, piecePos.y + py] = piece[px, py];
                }
            }
            SpawnPiece();
        }
    }

    public override void OnKeyDown(KeyboardKeyEventArgs e) {
        base.OnKeyDown(e);
        switch (e.Key) {
            case Keys.Left:
                if (CanMove(-1, 0)) {
                    piecePos.x--;
                }
                break;
            case Keys.Right:
                if (CanMove(1, 0)) {
                    piecePos.x++;
                }
                break;
            case Keys.Down:
                if (CanMove(0, 1)) {
                    piecePos.y++;
                }
                break;
            case Keys.Up:
                if (CanRotate()) {
                    piece = Rotate(piece);
                }
                break;
            case Keys.Space:
                if (CanRotate()) {
                    piece = Rotate(piece);
                }
                break;
            case Keys.Enter:
                while (CanMove(0, 1)) Drop();
                break;
            case Keys.Escape:
                OnExit();
                break;
        }
    }

    public override void OnJoystickButtonPressed(XInputMapping button) {
        base.OnJoystickButtonPressed(button);
        switch (button) {
            case XInputMapping.UpDPad:
                if (CanRotate()) {
                    piece = Rotate(piece);
                }
                break;
            case XInputMapping.DownDPad:
                if (CanMove(0, 1)) {
                    piecePos.y++;
                }
                break;
            case XInputMapping.LeftDPad:
                if (CanMove(-1, 0)) {
                    piecePos.x--;
                }
                break;
            case XInputMapping.RightDPad:
                if (CanMove(1, 0)) {
                    piecePos.x++;
                }
                break;
            case XInputMapping.A:
                while (CanMove(0, 1)) Drop();
                break;
            case XInputMapping.B:
                OnExit();
                break;
        }
    }

    public override void Update() {
        base.Update();
        int lines = 0;
        // Check for full rows
        for (int y = 0; y < BOARD_HEIGHT; y++) {
            var full = true;
            for (int x = 1; x < BOARD_WIDTH; x++) {
                if (board[x, y] == PieceType.E) {
                    full = false;
                    break;
                }
            }
            if (full) {
                lines++;
                for (int x = 0; x < BOARD_WIDTH; x++) {
                    board[x, y] = PieceType.E;
                }
                for (int ny = y; ny > 0; ny--) {
                    for (int x = 0; x < BOARD_WIDTH; x++) {
                        board[x, ny] = board[x, ny - 1];
                    }
                }
            }
        }
        if (lines > 0) {
            score += lines switch {
                1 => 40,
                2 => 100,
                3 => 300,
                4 => 1200,
                _ => 0
            };
        }

        timeout.Milliseconds = score > 7500 ? 100 :
                score > 5000 ? 200 :
                score > 2000 ? 300 :
                score > 1500 ? 400 :
                score > 1000 ? 500 :
                score > 500 ? 600 :
                score > 100 ? 700 : 800;
    }

    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.CenterScreen(WIDTH, HEIGHT);
        canvas.Translate(-CELL_SIZE, 0);
        for (int y = 0; y < BOARD_HEIGHT; y++) {
            for (int x = 0; x < BOARD_WIDTH; x++) {
                if (board[x, y] == PieceType.E) {
                    continue;
                }
                BlockPaint.Color = board[x, y].GetColor();
                canvas.DrawRect(x * CELL_SIZE, y * CELL_SIZE, CELL_SIZE, CELL_SIZE, BlockPaint);
            }
        }
        for (int py = 0; py < 4; py++) {
            for (int px = 0; px < 4; px++) {
                if (piece[px, py] == PieceType.E) {
                    continue;
                }
                BlockPaint.Color = piece[px, py].GetColor();
                canvas.DrawRect((piecePos.x + px) * CELL_SIZE, (piecePos.y + py) * CELL_SIZE, CELL_SIZE, CELL_SIZE, BlockPaint);
            }
        }
        canvas.Translate(CELL_SIZE, 0);
        canvas.DrawRect(0, 0, WIDTH, HEIGHT, BorderPaint);
        canvas.Translate(WIDTH + 15, 20);
        canvas.DrawText($"Score: {score}", 0, 0, ScorePaint);
    }


    private void SpawnPiece() {
        piece = ((PieceType)rng.Next(1, 8)).Generate();
        piecePos = (4, 2);
        if (!CanMove(0, 1)) {
            OnGameOver(score);
        }
    }


    private bool CanMove(int x, int y) {
        for (int py = 0; py < 4; py++) {
            for (int px = 0; px < 4; px++) {
                if (piece[px, py] == PieceType.E) {
                    continue;
                }
                var nx = piecePos.x + px + x;
                var ny = piecePos.y + py + y;
                if (nx < 1 || nx >= BOARD_WIDTH || ny < 0 || ny >= BOARD_HEIGHT) {
                    return false;
                }
                if (board[nx, ny] != PieceType.E) {
                    return false;
                }
            }
        }
        return true;
    }

    private bool CanRotate() {
        var rotated = Rotate(piece);
        for (int py = 0; py < 4; py++) {
            for (int px = 0; px < 4; px++) {
                if (rotated[px, py] == PieceType.E) {
                    continue;
                }
                var nx = piecePos.x + px;
                var ny = piecePos.y + py;
                if (nx < 1 || nx >= BOARD_WIDTH || ny < 0 || ny >= BOARD_HEIGHT) {
                    return false;
                }
                if (board[nx, ny] != PieceType.E) {
                    return false;
                }
            }
        }
        return true;
    }

    private PieceType[,] Rotate(PieceType[,] piece) {
        var rotated = new PieceType[4, 4];
        for (int y = 0; y < 4; y++) {
            for (int x = 0; x < 4; x++) {
                rotated[x, y] = piece[3 - y, x];
            }
        }
        return rotated;
    }

    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);
        ScorePaint.Dispose();
        BorderPaint.Dispose();
        BlockPaint.Dispose();
    }


}