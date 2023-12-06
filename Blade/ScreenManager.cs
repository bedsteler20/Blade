namespace Blade;

/// <summary>
/// A static class that manages the screens in the game.
/// </summary>
public static class ScreenManager {
    private static readonly Stack<Screen> screens = new();

    /// <summary>
    /// Gets the current screen being displayed.
    /// </summary>
    public static Screen CurrentScreen => screens.Peek();

    /// <summary>
    /// Adds a screen to the top of the screen stack.
    /// </summary>
    /// <param name="screen">The screen to add.</param>
    public static void AddScreen(Screen screen) {
        screens.Push(screen);
    }

    /// <summary>
    /// Removes the top screen from the screen stack.
    /// </summary>
    public static void Back() {
        CurrentScreen.Dispose();
        screens.Pop();
    }

    /// <summary>
    /// Navigates back to the previous screen of type T in the screen stack.
    /// </summary>
    /// <typeparam name="T">The type of the screen to navigate back to.</typeparam>
    public static void Back<T>() {
        while (CurrentScreen.GetType() != typeof(T)) {
            Back();
        }
    }

    /// <summary>
    /// Runs the screen manager, which updates and draws the current screen and listens for key presses.
    /// </summary>
    public static void Run() {
        // This is a hack to make sure the console is reset when the program exits
        AppDomain.CurrentDomain.ProcessExit += new EventHandler((sender, e) => {
            Console.CursorVisible = true;
            Console.ResetColor();
            Console.Clear();
        });

        Task consoleKeyTask = Task.Run(() => {
            while (true) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                CurrentScreen.OnKeyPress(key);
            }
        });
        while (true) {
            CurrentScreen.Update();
            Task.Run(TryDraw);
            Thread.Sleep(CurrentScreen.UpdateRate);
        }
    }

    private static ToSmallMessage errScreen = new();
    private static void TryDraw() {
        Console.Clear();
        try {
            CurrentScreen.Draw();
        } catch (ArgumentOutOfRangeException err) {
            // This is a hack to fix a bug where the console would crash when the window was resized
            // while the game was running
            const string CATCH_MSG = "The value must be greater than or equal to zero and less than the console's buffer size in that dimension";
            if (err.Message.Contains(CATCH_MSG)) {
                errScreen.Draw();
                var pw = Console.WindowWidth;
                var ph = Console.WindowHeight;
                while (Console.WindowWidth == pw || Console.WindowHeight == ph) {
                    Thread.Sleep(100);
                }
                TryDraw();
            } else {
                throw;
            }

        }
    }
}

class ToSmallMessage : Screen {
    public override int UpdateRate => 1000 / 10;
    const string MESSAGE = "The window is too small!";
    public override (int, int) Offset => GetCenter(MESSAGE.Length, 1);
    public override void Draw() {
        base.Draw();
        Paint(0, 0, MESSAGE, ConsoleColor.Black, ConsoleColor.Red);
    }
}