namespace Tetris;



public class Game : Blade.Screen {
    private const int BOARD_WIDTH = 18;
    private const int BORD_HEIGHT = 28;
    private const int WIDTH = BOARD_WIDTH * 3 - 2;
    private const int HEIGHT = BORD_HEIGHT - 2;

    public override (int x, int y) Offset => GetCenter(WIDTH, HEIGHT, -3, 0);
    public override int UpdateRate => 1000 / 20;
    private readonly Random rng = Blade.Utils.CreateRadom();
    private readonly Blade.AudioPlayer player = new();
    private readonly Blade.Leaderboard leaderboard;

    private PieceType[,] board = new PieceType[BOARD_WIDTH, BORD_HEIGHT];
    private PieceType[,] piece = new PieceType[4, 4];
    private (int x, int y) piecePos = (4, 2);
    private int score = 0;
    private int frame = 0;
    private int speed = 8;

    public Game(Blade.Leaderboard leaderboard) {
        this.leaderboard = leaderboard;
        SpawnPiece();
        player.Repeat = true;
        player.Play(Blade.Assets.GetPath("Tetris.ogg"));
    }

    public override void Draw() {
        base.Draw();
        Paint(3, 0, $"Score: {score}");
        DrawBorder(WIDTH, HEIGHT, 3, 1);

        for (int y = 0; y < BORD_HEIGHT; y++) {
            for (int x = 0; x < BOARD_WIDTH; x++) {
                var type = board[x, y];
                if (type == PieceType.E) {
                    continue;
                }
                var color = type.GetColor();
                Paint(x * 3 + 2, y, "███", color);
            }
        }

        for (int y = 0; y < 4; y++) {
            for (int x = 0; x < 4; x++) {
                var type = piece[x, y];
                if (type == PieceType.E) {
                    continue;
                }
                var color = type.GetColor();
                Paint((piecePos.x + x) * 3 + 2, piecePos.y + y, "███", color);

            }
        }
    }

    public override void Update() {
        base.Update();
        frame++;
        if (CanMove(0, 1)) {
            if (frame % speed == 0) {
                piecePos.y++;
            }
        } else {
            for (int py = 0; py < 4; py++) {
                for (int px = 0; px < 4; px++) {
                    if (piece[px, py] == PieceType.E) {
                        continue;
                    }
                    var nx = piecePos.x + px;
                    var ny = piecePos.y + py;
                    board[nx, ny] = piece[px, py];
                }
            }
            SpawnPiece();
        }
        int lines = 0;
        // Check for full rows
        for (int y = 0; y < BORD_HEIGHT; y++) {
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

        speed = score > 7500 ? 1 :
                score > 5000 ? 2 :
                score > 2000 ? 3 :
                score > 1500 ? 4 :
                score > 1000 ? 5 :
                score > 500 ? 6 :
                score > 100 ? 7 : 8;
    }

    public override void OnKeyPress(ConsoleKeyInfo key) {
        base.OnKeyPress(key);
        switch (key.Key) {
            case ConsoleKey.LeftArrow:
                if (CanMove(-1, 0)) {
                    piecePos.x--;
                }
                break;
            case ConsoleKey.RightArrow:
                if (CanMove(1, 0)) {
                    piecePos.x++;
                }
                break;
            case ConsoleKey.DownArrow:
                if (CanMove(0, 1)) {
                    piecePos.y++;
                }
                break;
            case ConsoleKey.UpArrow:
                if (CanRotate()) {
                    piece = Rotate(piece);
                }
                break;
            case ConsoleKey.Escape:
                Blade.ScreenManager.Back<Menu>();
                break;
            case ConsoleKey.Enter:
                while (CanMove(0, 1)) {
                    piecePos.y++;
                }
                break;
            case ConsoleKey.Spacebar:
                if (CanRotate()) {
                    piece = Rotate(piece);
                }
                break;
        }
    }

    private void GameOver() {
        player.Stop();
        Blade.ScreenManager.AddScreen(new Blade.GameOver() {
            OnGameOver = () => Blade.ScreenManager.AddScreen(new Blade.TextBox() {
                BackgroundColor = ConsoleColor.Cyan,
                OnSubmit = (sender, name) => {
                    leaderboard.AddScore(name, score);
                    leaderboard.Save();
                    Blade.ScreenManager.Back<Menu>();
                },
                OnCancel = Blade.ScreenManager.Back<Menu>
            }),
        });
    }

    private void SpawnPiece() {
        piece = ((PieceType)rng.Next(1, 8)).Generate();
        piecePos = (4, 2);
        if (!CanMove(0, 1)) {
            GameOver();
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
                if (nx < 1 || nx >= BOARD_WIDTH || ny < 0 || ny >= BORD_HEIGHT) {
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
                if (nx < 1 || nx >= BOARD_WIDTH || ny < 0 || ny >= BORD_HEIGHT) {
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



    public override void Dispose() {
        base.Dispose();
        player.Stop();
        player.Dispose();
    }
}
