// See https://aka.ms/new-console-template for more information
using Blade.Colors;
using Example.NumberGame;
using SkiaSharp;
using Blade.GUI;

class Program {
    public static void Main() {
        var mainMenu = new Menu(
            title: "Main Menu",
            backgroundColor: Catppuccin.Yellow,
            buttons: new[] {
                 new Button(
                    text: "Number Game",
                    onClick: () => Blade.ScreenManager.AddScreen(new Game())
                ),
                new Button(
                    text: "Exit",
                    onClick: () => Environment.Exit(0)
                ),

                new Button(
                    text: "Exit",
                    onClick: () => Environment.Exit(0)
                ),
                new Button(
                    text: "Exit",
                    onClick: () => Environment.Exit(0)
                ),

                new Button(
                    text: "Exit",
                    onClick: () => Environment.Exit(0)
                )
            }
        );
        var window = new Blade.Window("Blade", 800, 600);
        Blade.ScreenManager.AddScreen(mainMenu);
        window.Run();
    }
}