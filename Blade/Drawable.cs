namespace Blade;

/// <summary>
/// Represents an abstract drawable object that can be rendered to the screen.
/// </summary>
public abstract class Drawable {

    /// <summary>
    /// Gets or sets the offset of the drawable object.
    /// </summary>
    public virtual (int x, int y) Offset { get; set; }
    public Drawable() { }


    /// <summary>
    /// Renders what this object represents to the screen. and then calls Draw() on all children.
    /// </summary>
    public virtual void Draw() { }

    /// <summary>
    /// Paints pixel to the screen. This is the lowest level of rendering.
    /// </summary>
    public void Paint(int x, int y, string c) {
        Console.SetCursorPosition(x + Offset.x, y + Offset.y);
        Console.Write(c);
    }

    /// <summary>
    /// Paints the specified character at the given position with the specified foreground color.
    /// </summary>
    /// <param name="x">The x-coordinate of the position to paint the character.</param>
    /// <param name="y">The y-coordinate of the position to paint the character.</param>
    /// <param name="c">The character to paint.</param>
    /// <param name="foreground">The foreground color to use.</param>
    public void Paint(int x, int y, string c, ConsoleColor foreground) {
        Console.ForegroundColor = foreground;
        Paint(x, y, c);
        Console.ResetColor();
    }



    /// <summary>
    /// Paints a character on the console at the specified position with the specified foreground and background colors.
    /// </summary>
    /// <param name="x">The x-coordinate of the position to paint the character.</param>
    /// <param name="y">The y-coordinate of the position to paint the character.</param>
    /// <param name="c">The character to paint.</param>
    /// <param name="foreground">The foreground color of the character.</param>
    /// <param name="background">The background color of the character.</param>
    public void Paint(int x, int y, string c, ConsoleColor foreground, ConsoleColor background) {
        Console.ForegroundColor = foreground;
        Console.BackgroundColor = background;
        Paint(x, y, c);
        Console.ResetColor();
    }

    /// <summary>
    /// Paints a single character at the specified position with the specified background color.
    /// </summary>
    /// <param name="x">The x-coordinate of the position to paint the character.</param>
    /// <param name="y">The y-coordinate of the position to paint the character.</param>
    /// <param name="background">The background color to use when painting the character.</param>
    public void Paint(int x, int y, ConsoleColor background) {
        Console.BackgroundColor = background;
        Paint(x, y, " ");
        Console.ResetColor();
    }


    /// <summary>
    /// Calculates the center coordinates of a rectangle given its width and height, relative to the console window.
    /// </summary>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <returns>A tuple containing the x and y coordinates of the center of the rectangle.</returns>
    public (int x, int y) GetCenter(int width, int height) {
        int x = (Console.WindowWidth - width) / 2;
        int y = (Console.WindowHeight - height) / 2;
        if (x % 2 == 1) {
            x--;
        }
        if (y % 2 == 1) {
            y--;
        }
        return (x, y);
    }

    public (int x, int y) GetCenter(int x, int y, int yOff, int xOff) {
        var (cx, cy) = GetCenter(x, y);
        return (cx + xOff, cy + yOff);
    }

    public void MoveCursor(int x, int y) {
        Console.SetCursorPosition(x + Offset.x, y + Offset.y);
    }

    public void PaintRect(int x1, int y1, int x2, int y2, string c) {
        for (int x = x1; x <= x2; x++) {
            for (int y = y1; y <= y2; y++) {
                Paint(x, y, c);
            }
        }
    }

    public void PaintRect(int x1, int y1, int x2, int y2, string c, ConsoleColor foreground) {
        for (int x = x1; x <= x2; x++) {
            for (int y = y1; y <= y2; y++) {
                Paint(x, y, c, foreground);
            }
        }
    }

    public void DrawBorder(int width, int height, int xOff = 0, int yOff = 0) {
        Paint(xOff, yOff, "┏");
        Paint(xOff, height + 1 + yOff, "┗");
        Paint(width + 1 + xOff, height + 1 + yOff, "┛");
        Paint(width + 1 + xOff, yOff, "┓");

        for (int x = xOff; x < width + xOff; x++) {
            Paint(x + 1, yOff, "━");
            Paint(x + 1, height + 1 + yOff, "━");
        }
        for (int y = yOff; y < height + yOff; y++) {
            Paint(xOff, y + 1, "┃");
            Paint(width + 1 + xOff, y + 1, "┃");
        }

    }
}