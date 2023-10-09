// See https://aka.ms/new-console-template for more information
using Blade.Colors;
using Blade.GUI;
using Blade;
using SkiaSharp;
namespace SharpGames;

class Program {


    public static Action BuildGame<T>(string routePrefix, string title, SKColor color, SKColor textColor) where T : GameScreen, new() {
        var leaderboard = new Leaderboard(routePrefix);
        return () => ScreenManager.AddScreen(new Menu() {
            Title = title,
            Route = $"/{routePrefix}",
            BackgroundColor = color,
            Buttons = new[] {
                new Button {
                    Text = "Play",
                    OnClick = () =>ScreenManager.AddScreen(new T() {
                        Leaderboard = leaderboard,
                        Route = $"/{routePrefix}/game",
                        OnGameOver = (score) => ScreenManager.AddScreen(new GameOver {
                            Route = $"/{routePrefix}/over",
                            OnEnd = () => ScreenManager.AddScreen(new TextBox {
                                Route = $"/{routePrefix}/name",
                                Title = "Enter your name",
                                BackgroundColor = color,
                                CancelText = "Cancel",
                                SubmitText = "Submit",
                                TextColor = textColor,
                                OnCancel = () => ScreenManager.Back($"/{routePrefix}"),
                                OnSubmit = (name) => {
                                    leaderboard.AddScore(name, score);
                                    ScreenManager.Back($"/{routePrefix}");
                                }
                            })
                        }),
                        OnExit = () => ScreenManager.AddScreen(new ConfirmDialog {
                            Message = "Exit Game?",
                            Route = $"/{routePrefix}/exit",
                            BackgroundColor = color,
                            TextColor = textColor,
                            CancelAction = () => ScreenManager.Back($"/{routePrefix}"),
                            ConfirmAction = () => ScreenManager.Back($"/{routePrefix}/game"),
                            ConfirmText = "No",
                            CancelText = "Yes"
                        }),
                    }),
                },
                new Button {
                    Text = "Leaderboard",
                    OnClick = () => ScreenManager.AddScreen(new Table {
                        Title = "Leaderboard",
                        Route = $"/{routePrefix}/scores",
                        Data = new Leaderboard(routePrefix).GetData(),
                        HeaderTextColor = textColor,
                        BackgroundColor = color
                    })
                },
                new Button {
                    Text = "Back",
                    OnClick = () => ScreenManager.Back("/")
                }
            }
        });
    }

    public static void Main() {
        Audio.Initialize();
        Catppuccin.Initialize();
        var numberGameLeaderboard = new Leaderboard("2048");
        var snakeLeaderboard = new Leaderboard("Snake");

        var menu = new Menu {
            Title = "Main Menu",
            Route = "/",
            BackgroundColor = Catppuccin.Pink,
            Buttons = new[] {
                new Button {
                    Text = "2048",
                    OnClick = BuildGame<NumberGame.Game>("2048", "2048", Catppuccin.Yellow, Catppuccin.Base)
                },
                new Button() {
                    Text = "Snake",
                    OnClick = BuildGame<Snake.Game>("Snake", "Snake", Catppuccin.Green, Catppuccin.Base)
                },
                new Button() {
                    Text = "Break Out",
                    OnClick = BuildGame<BreakOut.Game>("BreakOut", "Break Out", Catppuccin.Blue, Catppuccin.Base)
                },
                new Button() {
                    Text = "Tetras",
                    OnClick = BuildGame<Tetras.Game>("Tetras", "Tetras", Catppuccin.Lavender, Catppuccin.Base)
                },
                new Button() {
                    Text = "Exit",
                    OnClick = () => Environment.Exit(0)
                }
}
        };
        var window = new Window("Blade", 800, 600);
        ScreenManager.AddScreen(menu);
        window.Run();
    }
}
