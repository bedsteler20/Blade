// See https://aka.ms/new-console-template for more information
using Blade.Colors;
using Blade.GUI;
using Blade;
using System.Text.Json;
namespace SharpGames;

class Program {
    public static void Main() {
        Catppuccin.Initialize();
        var numberGameLeaderboard = new Leaderboard("2048");
        var snakeLeaderboard = new Leaderboard("Snake");

        var menu = new Menu {
            Title = "Main Menu",
            Route = Routes.MainMenu,
            BackgroundColor = Catppuccin.Pink,
            Buttons = new[] {
                new Button {
                    Text = "2048",
                    OnClick = () => ScreenManager.AddScreen(new Menu() {
                        Title = "2048",
                        Route = Routes.NumbersGameMenu,
                        BackgroundColor = Catppuccin.Yellow,
                        Buttons = new[] {
                            new Button {
                                Text = "Play",
                                OnClick = () => ScreenManager.AddScreen(new NumberGame.Game {
                                    Route = Routes.NumbersGame,
                                    Leaderboard = numberGameLeaderboard,
                                    OnExit = () => ScreenManager.AddScreen(new ConfirmDialog {
                                        Message = "Exit Game?",
                                        Route = Routes.NumbersGameExitConfirm,
                                        BackgroundColor = Catppuccin.Yellow,
                                        TextColor = Catppuccin.Base,
                                        CancelAction  = () => ScreenManager.Back(Routes.NumbersGameMenu),
                                        ConfirmAction = () => ScreenManager.Back(Routes.NumbersGame),
                                        ConfirmText = "Yes",
                                        CancelText = "No"
                                    }),
                                    OnGameOver = (score) => ScreenManager.AddScreen(new GameOver {
                                        Route = Routes.NumbersGameOver,
                                        OnEnd = () => new TextBox {
                                            Route = Routes.NumbersGameName,
                                            Title = "Enter your name",
                                            BackgroundColor = Catppuccin.Yellow,
                                            CancelText = "Cancel",
                                            SubmitText = "Submit",
                                            TextColor = Catppuccin.Base,
                                            OnCancel = () => ScreenManager.Back(Routes.NumbersGameMenu),
                                            OnSubmit = (name) => {
                                                snakeLeaderboard.AddScore(name, score);
                                                ScreenManager.Back(Routes.NumbersGameMenu);
                                            }
                                        },
                                    })                                })
                            },
                            new Button {
                                Text = "Leaderboard",
                                OnClick = () => ScreenManager.AddScreen(new Table {
                                    Title = "Leaderboard",
                                    Route = Routes.NumbersGameScores,
                                    Data = numberGameLeaderboard.GetData(),
                                    HeaderTextColor = Catppuccin.Base,
                                    BodyTextColor = Catppuccin.Text,
                                    BackgroundColor = Catppuccin.Yellow
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
                        BackgroundColor = Catppuccin.Green,
                        Buttons = new[] {
                            new Button {
                                Text = "Play",
                                OnClick = () => ScreenManager.AddScreen(new NumberGame.Game {
                                    Route = Routes.SnakeGame,
                                    Leaderboard = snakeLeaderboard,
                                    OnExit = () => ScreenManager.AddScreen(new ConfirmDialog {
                                        Message = "Exit Game?",
                                        Route = Routes.SnakeExitConfirm,
                                        BackgroundColor = Catppuccin.Green,
                                        TextColor = Catppuccin.Base,
                                        ConfirmAction = () => ScreenManager.Back(Routes.SnakeMenu),
                                        CancelAction = () => ScreenManager.Back(Routes.SnakeGame),
                                        ConfirmText = "Yes",
                                        CancelText = "No"
                                    }),
                                    OnGameOver = (score) => ScreenManager.AddScreen(new GameOver {
                                        Route = Routes.SnakeOver,
                                        OnEnd = () => ScreenManager.AddScreen(
                                            new TextBox {
                                                Route = Routes.SnakeName,
                                                BackgroundColor = Catppuccin.Yellow,
                                                Title = "Enter your name",
                                                CancelText = "Cancel",
                                                SubmitText = "Submit",
                                                TextColor = Catppuccin.Base,
                                                OnCancel = () => ScreenManager.Back(Routes.SnakeMenu),
                                                OnSubmit = (name) => {
                                                snakeLeaderboard.AddScore(name, score);
                                                ScreenManager.Back(Routes.SnakeMenu);
                                            }
                                        })
                                    })
                                })
                            },
                            new Button {
                                Text = "Leaderboard",
                                OnClick = () => ScreenManager.AddScreen(new Table {
                                    Title = "Leaderboard",
                                    Route = Routes.SnakeScores,
                                    Data = snakeLeaderboard.GetData(),
                                    HeaderTextColor = Catppuccin.Base,
                                    BodyTextColor = Catppuccin.Text,
                                    BackgroundColor = Catppuccin.Green
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
                    OnClick = () => { }
                },
                new Button() {
        Text = "Tetras",
                    OnClick = () => { }
                },
                new Button() {
        Text = "Exit",
                    OnClick = () => Environment.Exit(0)
                }
}
        };
        var window = new Window("Blade", 800, 600);
        // ScreenManager.AddScreen(menu);
        ScreenManager.AddScreen(new TextBox {
            Route = Routes.SnakeName,
            BackgroundColor = Catppuccin.Yellow,
            Title = "Enter your name",
            CancelText = "Cancel",
            SubmitText = "Submit",
            TextColor = Catppuccin.Base,
            OnCancel = () => ScreenManager.Back(Routes.SnakeMenu),
            OnSubmit = (name) => {
                snakeLeaderboard.AddScore(name, 1);
                ScreenManager.Back(Routes.SnakeMenu);
            }
        });
        window.Run();
    }
}
