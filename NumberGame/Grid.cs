using System;

namespace NumberGame;


class Grid : Blade.Drawable {
    Cell[][] Cells;
    public Grid(ref Cell[][] cells) {
        Cells = cells; Offset = (0, 0);

    }
    const int WIDTH = 50;
    const int HEIGHT = 18;
    public override (int x, int y) Offset => GetCenter(WIDTH, HEIGHT);

    public override void Draw() {
        // I have no idea how what any of this numbers do but it works
        for (int x = 0; x < Constraints.BORD_SIZE; x++) {
            for (int y = 0; y < Constraints.BORD_SIZE; y++) {
                if (y == 0) {
                    if (x == 0) {
                        Paint(x * 12, y * 4 + 1, "┏━━━━━━━━━━━┳━");
                    } else if (x == Constraints.BORD_SIZE - 1) {
                        Paint(x * 12 + 1, y * 4 + 1, "━━━━━━━━━━━┓ ");
                    } else {
                        Paint(x * 12 + 1, y * 4 + 1, "━━━━━━━━━━━┳━");
                    }

                } else {
                    if (x == 0) {
                        Paint(x * 12, y * 4 + 1, "┣━━━━━━━━━━━╋━");
                    } else if (x == Constraints.BORD_SIZE - 1) {
                        Paint(x * 12 + 1, y * 4 + 1, "━━━━━━━━━━━┫ ");
                    } else {
                        Paint(x * 12 + 1, y * 4 + 1, "━━━━━━━━━━━╋━");
                    }

                    if (y == Constraints.BORD_SIZE - 1) {
                        if (x == 0) {
                            Paint(x * 12, y * 4 + 5, "┗━━━━━━━━━━━┻━");
                        } else if (x == Constraints.BORD_SIZE - 1) {
                            Paint(x * 12 + 1, y * 4 + 5, "━━━━━━━━━━━┛ ");
                        } else {
                            Paint(x * 12 + 1, y * 4 + 5, "━━━━━━━━━━━┻━");
                        }
                    }
                }
                Paint(x * 12, y * 4 + 2, "┃");
                Paint(x * 12, y * 4 + 3, "┃");
                Paint(x * 12, y * 4 + 4, "┃");
                Cells[x][y].Offset = ((x * 6) + (Offset.x / 2) + 1, (y * 2) + (Offset.y / 2) + 1);
                Cells[x][y].Draw();
                Paint(x * 12 + 12, y * 4 + 2, "┃");
                Paint(x * 12 + 12, y * 4 + 3, "┃");
                Paint(x * 12 + 12, y * 4 + 4, "┃");

            }

        }
    }
}