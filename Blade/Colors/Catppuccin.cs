namespace Blade.Colors;

using System.Text.Json;
using SkiaSharp;

public class Catppuccin {
    private static readonly SKColor empty = SKColor.Empty;
    public static SKColor Empty = empty;
    public static SKColor Rosewater;
    public static SKColor Flamingo;
    public static SKColor Pink;
    public static SKColor Mauve;
    public static SKColor Red;
    public static SKColor Maroon;
    public static SKColor Peach;
    public static SKColor Yellow;
    public static SKColor Green;
    public static SKColor Teal;
    public static SKColor Sky;
    public static SKColor Sapphire;
    public static SKColor Blue;
    public static SKColor Lavender;
    public static SKColor Text;
    public static SKColor Subtext1;
    public static SKColor Subtext0;
    public static SKColor Overlay2;
    public static SKColor Overlay1;
    public static SKColor Overlay0;
    public static SKColor Surface2;
    public static SKColor Surface1;
    public static SKColor Surface0;
    public static SKColor Base;
    public static SKColor Mantle;
    public static SKColor Crust;
    public static SKTypeface Font;

    public static void Initialize() {
        var themePath = Assets.GetPath("Themes", EngineFlags.Theme + ".json");
        if (!File.Exists(themePath)) {
            themePath = Assets.GetPath("Themes", "Mocha.json");
        }
        var theme = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(themePath));
        Rosewater = SKColor.Parse(theme["Rosewater"]);
        Flamingo = SKColor.Parse(theme["Flamingo"]);
        Pink = SKColor.Parse(theme["Pink"]);
        Mauve = SKColor.Parse(theme["Mauve"]);
        Red = SKColor.Parse(theme["Red"]);
        Maroon = SKColor.Parse(theme["Maroon"]);
        Peach = SKColor.Parse(theme["Peach"]);
        Yellow = SKColor.Parse(theme["Yellow"]);
        Green = SKColor.Parse(theme["Green"]);
        Teal = SKColor.Parse(theme["Teal"]);
        Sky = SKColor.Parse(theme["Sky"]);
        Sapphire = SKColor.Parse(theme["Sapphire"]);
        Blue = SKColor.Parse(theme["Blue"]);
        Lavender = SKColor.Parse(theme["Lavender"]);
        Text = SKColor.Parse(theme["Text"]);
        Subtext1 = SKColor.Parse(theme["Subtext1"]);
        Subtext0 = SKColor.Parse(theme["Subtext0"]);
        Overlay2 = SKColor.Parse(theme["Overlay2"]);
        Overlay1 = SKColor.Parse(theme["Overlay1"]);
        Overlay0 = SKColor.Parse(theme["Overlay0"]);
        Surface2 = SKColor.Parse(theme["Surface2"]);
        Surface1 = SKColor.Parse(theme["Surface1"]);
        Surface0 = SKColor.Parse(theme["Surface0"]);
        Base = SKColor.Parse(theme["Base"]);
        Mantle = SKColor.Parse(theme["Mantle"]);
        Crust = SKColor.Parse(theme["Crust"]);
    }
}