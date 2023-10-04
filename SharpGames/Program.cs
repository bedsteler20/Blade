// See https://aka.ms/new-console-template for more information
using Blade.Colors;
using Blade.GUI;
using Blade;
using System.Text.Json;
namespace SharpGames;

class Program {
    public static void Main() {
        Dictionary<string, uint> pairs = new() {
            ["Rosewater"] = 0xfff5e0dc
        };
        Console.WriteLine(JsonSerializer.Serialize(pairs));
        var numberGameLeaderboard = new Leaderboard("2048");
        var snakeLeaderboard = new Leaderboard("Snake");

        var menu = new Menu {
            Title = "Main Menu",
            Route = Routes.MainMenu,
            BackgroundColor = CatppuccinMocha.Pink,
            Buttons = new[] {
                new Button {
                    Text = "2048",
                    OnClick = () => ScreenManager.AddScreen(new Menu() {
                        Title = "2048",
                        Route = Routes.NumbersGameMenu,
                        BackgroundColor = CatppuccinMocha.Yellow,
                        Buttons = new[] {
                            new Button {
                                Text = "Play",
                                OnClick = () => ScreenManager.AddScreen(new NumberGame.Game {
                                    Route = Routes.NumbersGame,
                                    Leaderboard = numberGameLeaderboard,
                                    OnExit = () => ScreenManager.AddScreen(new ConfirmDialog {
                                        Message = "Exit Game?",
                                        Route = Routes.NumbersGameExitConfirm,
                                        BackgroundColor = CatppuccinMocha.Yellow,
                                        TextColor = CatppuccinMocha.Base,
                                        ConfirmAction = () => ScreenManager.Back(Routes.NumbersGameMenu),
                                        CancelAction = () => ScreenManager.Back(Routes.NumbersGame),
                                        ConfirmText = "Yes",
                                        CancelText = "No"
                                    }),
                                    OnGameOver = (score) => { }
                                })
                            },
                            new Button {
                                Text = "Leaderboard",
                                OnClick = () => ScreenManager.AddScreen(new Table {
                                    Title = "Leaderboard",
                                    Route = Routes.NumbersGameScores,
                                    Data = numberGameLeaderboard.GetData(),
                                    HeaderTextColor = CatppuccinMocha.Base,
                                    BodyTextColor = CatppuccinMocha.Text,
                                    BackgroundColor = CatppuccinMocha.Yellow
                                })
                            },
                            new Button {
                                Text = "Back",
                                OnClick = () => ScreenManager.Back(Routes.MainMenu)
                            }
                        }
                    })
                },
                new Button() {
                    Text = "Snake",
                    OnClick = () => ScreenManager.AddScreen(new Menu() {
                        Title = "Snake",
                        Route = Routes.SnakeMenu,
                        BackgroundColor = CatppuccinMocha.Green,
                        Buttons = new[] {
                            new Button {
                                Text = "Play",
                                OnClick = () => ScreenManager.AddScreen(new NumberGame.Game {
                                    Route = Routes.SnakeGame,
                                    Leaderboard = snakeLeaderboard,
                                    OnExit = () => ScreenManager.AddScreen(new ConfirmDialog {
                                        Message = "Exit Game?",
                                        Route = Routes.SnakeExitConfirm,
                                        BackgroundColor = CatppuccinMocha.Green,
                                        TextColor = CatppuccinMocha.Base,
                                        ConfirmAction = () => ScreenManager.Back(Routes.SnakeMenu),
                                        CancelAction = () => ScreenManager.Back(Routes.SnakeGame),
                                        ConfirmText = "Yes",
                                        CancelText = "No"
                                    }),
                                    OnGameOver = (score) => { }
                                })
                            },
                            new Button {
                                Text = "Leaderboard",
                                OnClick = () => ScreenManager.AddScreen(new Table {
                                    Title = "Leaderboard",
                                    Route = Routes.SnakeScores,
                                    Data = snakeLeaderboard.GetData(),
                                    HeaderTextColor = CatppuccinMocha.Base,
                                    BodyTextColor = CatppuccinMocha.Text,
                                    BackgroundColor = CatppuccinMocha.Green
                                })
                            },
                            new Button {
                                Text = "Back",
                                OnClick = () => ScreenManager.Back(Routes.MainMenu)
                            }
                        }
                    })
                },
                new Button() {
                    Text = "BreakOut",
                    OnClick = () => {}
                },
                new Button() {
                    Text = "Tetras",
                    OnClick = () => {}
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
