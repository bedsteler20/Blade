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

    public static int Count => screens.Count;

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
    /// Navigates back to the screen with the specified name.
    /// </summary>
    /// <param name="name">The name of the screen to navigate back to.</param>
    public static void Back(string name) {
        while (CurrentScreen is not null && CurrentScreen.Route != name) {
            Back();
        }
    }
}
