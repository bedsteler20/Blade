namespace Blade;

public static class Utils {
    public static string GetAppFile(string appName, string file) {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appDir = Path.Combine(appData, "Blade", appName);
        if (!Directory.Exists(appDir)) {
            Directory.CreateDirectory(appDir);
        }
        return Path.Combine(appDir, file);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Random"/> class with a seed value specified by the "BLADE_RANDOM_SEED" environment variable, if it exists.
    /// If the environment variable is not set, a new instance of the <see cref="Random"/> class is created with a seed value based on the current time.
    /// </summary>
    /// <returns>A new instance of the <see cref="Random"/> class.</returns>
    public static Random CreateRadom() {
        var seed = EngineFlags.RandomSeed;
        if (seed == -1) {
            return new Random();
        } else {
            return new Random(seed);
        }
    }
}

public static class EngineFlags {
    public static bool CollectGarbageAfterFrame { get; set; } = GetVar("COLLECT_GARBAGE_AFTER_FRAME", true);
    public static int FrameRate { get; set; } = GetVar("FRAME_RATE", 60);
    public static bool UseVsync { get; set; } = GetVar("USE_VSYNC", true);
    public static string Theme { get; set; } = GetVar("THEME", "Mocha");
    public static bool UseSystemFont { get; set; } = GetVar("USE_SYSTEM_FONT", false);
    public static string Font { get; set; } = GetVar("FONT", "ComicMono");
    public static int RandomSeed { get; set; } = GetVar("RANDOM_SEED", -1);

    private static bool GetVar(string name, bool defaultValue = false) {
        var value = Environment.GetEnvironmentVariable("BLADE_" + name);
        if (value == null) {
            return defaultValue;
        }
        return value.ToLower() == "true" || value == "1";
    }

    private static int GetVar(string name, int defaultValue = 0) {
        var value = Environment.GetEnvironmentVariable("BLADE_" + name);
        if (value == null) {
            return defaultValue;
        }
        if (int.TryParse(value, out var result)) {
            return result;
        }
        return defaultValue;
    }

    private static string GetVar(string name, string defaultValue = "") {
        var value = Environment.GetEnvironmentVariable("BLADE_" + name);
        if (value == null) {
            return defaultValue;
        }
        return value;
    }
}