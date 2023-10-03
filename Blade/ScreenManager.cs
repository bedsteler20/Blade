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
}
