using Blade.Colors;
using Blade.Extensions;
using SkiaSharp;

namespace Blade.GUI;

public class GameOver : Screen {
    public Action OnEnd { get; init; }

    private readonly SKPaint Paint = new() {
        Color = Catppuccin.Red,
        TextAlign = SKTextAlign.Center,
        IsAntialias = true,
        TextSize = 54,
        Typeface = Catppuccin.Font
    };

    protected override void Setup() {
        base.Setup();
        Task.Run(async () => {
            await Task.Delay(1500);
            OnEnd();
        });
    }

    protected override void OnDraw(SKCanvas canvas) {
        base.OnDraw(canvas);
        canvas.CenterScreen(0, 0);
        canvas.DrawText("Game Over", 0, 0, Paint);
    }
}