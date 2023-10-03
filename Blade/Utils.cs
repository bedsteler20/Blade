namespace Blade;

public static class Utils {
   
}

public static class EngineFlags {
    public static bool CollectGarbageAfterFrame { get; set; } = GetVar("COLLECT_GARBAGE_AFTER_FRAME", true);
    public static int FrameRate {get; set;} = GetVar("FRAME_RATE", 60);

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
        _ = int.TryParse(value, out defaultValue);
        return defaultValue;
    }
}