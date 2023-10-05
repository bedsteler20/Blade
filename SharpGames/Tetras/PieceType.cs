using Blade.Colors;
using SkiaSharp;

namespace SharpGames.Tetras;

public enum PieceType {
    E = 0,
    I = 1,
    O = 2,
    T = 3,
    S = 4,
    Z = 5,
    J = 6,
    L = 7
}

public static class PieceTypeExtensions {
    public static SKColor GetColor(this PieceType type) => type switch {
        PieceType.E => Catppuccin.Empty,
        PieceType.I => Catppuccin.Sky,
        PieceType.O => Catppuccin.Yellow,
        PieceType.T => Catppuccin.Lavender,
        PieceType.S => Catppuccin.Green,
        PieceType.Z => Catppuccin.Red,
        PieceType.J => Catppuccin.Blue,
        PieceType.L => Catppuccin.Pink,
        _ => throw new ArgumentException("Invalid piece type")
    };

    public static PieceType[,] Generate(this PieceType type) => type switch {
        PieceType.E => new PieceType[,] {
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E }
        },
        PieceType.I => new PieceType[,] {
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.I, PieceType.I, PieceType.I, PieceType.I },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E }
        },
        PieceType.O => new PieceType[,] {
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.E, PieceType.O, PieceType.O, PieceType.E },
            { PieceType.E, PieceType.O, PieceType.O, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E }
        },
        PieceType.T => new PieceType[,] {
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.E, PieceType.T, PieceType.E, PieceType.E },
            { PieceType.T, PieceType.T, PieceType.T, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E }
        },
        PieceType.S => new PieceType[,] {
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.E, PieceType.S, PieceType.S, PieceType.E },
            { PieceType.S, PieceType.S, PieceType.E, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E }
        },
        PieceType.Z => new PieceType[,] {
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.Z, PieceType.Z, PieceType.E, PieceType.E },
            { PieceType.E, PieceType.Z, PieceType.Z, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E }
        },
        PieceType.J => new PieceType[,] {
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.J, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.J, PieceType.J, PieceType.J, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E }
        },
        PieceType.L => new PieceType[,] {
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.L, PieceType.E },
            { PieceType.L, PieceType.L, PieceType.L, PieceType.E },
            { PieceType.E, PieceType.E, PieceType.E, PieceType.E }
        },
        _ => throw new ArgumentException("Invalid piece type")
    };

}