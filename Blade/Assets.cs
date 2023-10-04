
public static class Assets {
    public static string GetPath(string path) {
        var asm = System.Reflection.Assembly.GetExecutingAssembly();
        string assemblyPath = Path.GetDirectoryName(asm.Location)!;
        return Path.Combine(assemblyPath, "Assets", path);
    }

    public static string GetPath(string folder, string file) => GetPath(Path.Combine(folder, file));
}