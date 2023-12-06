namespace NumberGame;


class Cell : Blade.Drawable {
    public int Value;

    public Cell(int value) {
        Value = value;
    }
    public ConsoleColor GetBackgroundColor() {
        if (Value == 0) {
            return Console.BackgroundColor;
        } else if (Value == 2) {
            return ConsoleColor.Yellow;
        } else if (Value == 4) {
            return ConsoleColor.Red;
        } else if (Value == 8) {
            return ConsoleColor.Green;
        } else if (Value == 16) {
            return ConsoleColor.Blue;
        } else if (Value == 32) {
            return ConsoleColor.Magenta;
        } else if (Value == 64) {
            return ConsoleColor.Cyan;
        } else if (Value == 128) {
            return ConsoleColor.Gray;
        } else if (Value == 256) {
            return ConsoleColor.Black;
        } else if (Value == 512) {
            return ConsoleColor.DarkYellow;
        } else if (Value == 1024) {
            return ConsoleColor.DarkGreen;
        } else if (Value == 2048) {
            return ConsoleColor.DarkBlue;
        }
        return ConsoleColor.Black;
    }

    public ConsoleColor GetForegroundColor() {
        if (Value == 0) {
            return Console.ForegroundColor;
        }
        return ConsoleColor.Black;
    }

    public string GetText() {
        if (Value == 0) {
            return new(' ', Constraints.CELL_CHAR_WIDTH);
        }
        return Blade.Utils.CenterText(Value.ToString(), Constraints.CELL_CHAR_WIDTH);
    }

    public override void Draw() {
        base.Draw();
        for (int lineNumber = 0; lineNumber < 3; lineNumber++) {
            if (lineNumber == 1) {
                Paint(Offset.x, lineNumber + Offset.y, GetText(), GetForegroundColor(), GetBackgroundColor());
                continue;
            }

            for (int i = 0; i < Constraints.CELL_CHAR_WIDTH; i++) {
                Paint(i + Offset.x, lineNumber + Offset.y, GetBackgroundColor());
            }
        }
    }

    public bool CanMerge(Cell other) {
        if (Value == 0 || other.Value == 0) {
            return true;
        }
        return Value == other.Value;
    }

    // Adds spaces to the left and right of the text to center it

}