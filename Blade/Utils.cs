namespace Blade;

public class Utils {
    /// <summary>
    /// Creates a new instance of the <see cref="Random"/> class with a seed value specified by the "BLADE_RANDOM_SEED" environment variable, if it exists.
    /// If the environment variable is not set, a new instance of the <see cref="Random"/> class is created with a seed value based on the current time.
    /// </summary>
    /// <returns>A new instance of the <see cref="Random"/> class.</returns>
    public static Random CreateRadom() {
        var seed = Environment.GetEnvironmentVariable("BLADE_RANDOM_SEED");
        if (seed == null) {
            return new Random();
        } else {
            return new Random(int.Parse(seed));
        }
    }

    public static string CenterText(string text, int width) {
        int textLength = text.Length;
        int spaces = width - textLength;
        int leftSpaces = spaces / 2;
        int rightSpaces = spaces - leftSpaces;
        string leftPadding = new(' ', leftSpaces);
        string rightPadding = new(' ', rightSpaces);
        return leftPadding + text + rightPadding;
    }

    public static string RightText(string text, int width) {
        int textLength = text.Length;
        int spaces = width - textLength;
        string padding = new(' ', spaces);
        return padding + text;
    }

    public static string GetAppFile(string appName, string file) {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appDir = Path.Combine(appData, "Blade", appName);
        if (!Directory.Exists(appDir)) {
            Directory.CreateDirectory(appDir);
        }
        return Path.Combine(appDir, file);
    }
}