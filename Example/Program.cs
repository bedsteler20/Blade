// See https://aka.ms/new-console-template for more information
using Example.NumberGame;

var window = new Blade.Window("Blade", 1920, 1080);
Blade.ScreenManager.AddScreen(new Game());
window.Run();