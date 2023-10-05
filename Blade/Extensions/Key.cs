using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Blade.Extensions;

public static class KeyExtension {
    public static bool IsPrintable(this Keys key) =>  key >= Keys.A && key <= Keys.Z;
}