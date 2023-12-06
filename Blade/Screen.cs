namespace Blade;

/// <summary>
/// Represents the update strategy for the game screen.
/// </summary>
public enum UpdateStrategy {
    /// <summary>
    /// Update the game at a fixed rate.
    /// </summary>
    FixedRate,
    /// <summary>
    /// Update the game after user input.
    /// </summary>
    Lazy,
}

/// <summary>
/// Represents a screen that can be drawn on the console and updated at a fixed rate or lazily.
/// </summary>
public abstract class Screen : Drawable {
    [Obsolete("All screens are now fixed rate.")]
    public virtual UpdateStrategy updateStrategy => UpdateStrategy.FixedRate;
    public virtual bool ShowCursor { get; } = false;
    public virtual int UpdateRate { get; } = 1000 / 60;


    public virtual void Update() {
        if (ShowCursor) {
            Console.CursorVisible = true;
        } else {
            Console.CursorVisible = false;
        }
    }

    /// <summary>
    /// Called when a key is pressed while the screen is active.
    /// </summary>
    /// <param name="key">The key that was pressed.</param>
    public virtual void OnKeyPress(ConsoleKeyInfo key) { }


    public virtual void Dispose() { }
}