The full source code for Blade can be found [hear](https://github.com/bedsteler20/Blade)


## Games and Controls
### General
`esc` will exit the game.

### Number Game
Uses the arrow keys to move the blocks around. The goal is to combine blocks of the same number to create a block with a higher number. The game ends when you can no longer move any blocks.

### Snake
Uses the arrow keys to move the snake around. The goal is to eat as many apples as possible without running into yourself or the walls. If you eat an apple, the snake will grow longer. Moving backwards will kill the snake.

### Breakout
Uses the arrow keys to move the paddle. Holding `shift` will move the prattle faster The goal is to break all of the blocks without letting the ball hit the bottom of the screen. If the ball hits the bottom of the screen, you lose a life. If you lose all of your lives, the game ends.

### Tetris
Uses the arrow keys to move the blocks. `space` will rotate the block and `enter` will drop the block.  The goal is to fill as many rows as possible. If you fill a row, it will be cleared and you will gain points. If you fill the screen, the game ends.

## Env Variables

###### BLADE_COLLECT_GARBAGE_AFTER_FRAME
Default: true
Description: Runs `gc.collect()` after each frame. This will slow down the game slightly, but will help with memory management.

###### BLADE_FRAME_RATE
Default: 60
Description: The frame rate of the game. This is the number of times the game will update per second.

###### BLADE_USE_VSYNC
Default: true
Description: Whether or not to use VSync. This will limit the frame rate to the refresh rate of the monitor.

###### BLADE_THEME
Default: Mocha
Builtin: Mocha, Latte, Frappe, Macchiato
Description: The theme to use for the game. This will change the colors of the game. Themes are stored as a json file in Assets/Themes.

###### BLADE_FONT
Default: ComicMono
Description: The font to use for the game. Fonts are stored as a json file in Assets/Fonts.

###### BLADE_RANDOM_SEED
Default: -1 (Creates a random seed)
Description: The seed to use for the random number generator. This will allow you to create the same random numbers each time you run the game.

## Technical Details

### The Engine
Blade is a game engine that uses a screen system to manage the different parts of the game. Each screen is a different part of the game. For example, the main menu is a screen, the game over screen is a screen, and the game itself is a screen. Switching between screens is done by the `ScreenManager`. The `ScreenManager` is responsible for updating and drawing the current screen. The `ScreenManager` also handles the transition between screens. 

Every screen is a `GameObject` which is the base class for all objects in the game. `GameObject`s are responsible for updating and drawing themselves. `GameObject`s can also have children `GameObject`s. This allows for easy grouping of objects. For example, the `ScreenManager` is a `GameObject` that has the current screen as a child. This allows the `ScreenManager` to update and draw the current screen.

### GUI
Blade uses a custom GUI system. The GUI system is based on the `GameObject` system. Each GUI element is a `GameObject`. Some of the current widgets are `Button`, `ConfirmDialog`, `GameOver`, `Table`, `TextBox`, and `Menu`. All of this elements are built directly into the engine. The GUI system is designed to be easily extensible.

### Rendering
Blade uses OpenTK for creating a window and drawing to the screen. It uses OpenGL for rendering. Individual parts of the games are rendered using SkiaSharp. This allows for easy drawing of shapes and text.

### Input
OpenTK handles keyboard and controller input and passes it to the current game screen.

XInput compatible controllers are supported. This has only been tested on linux with the Xbox Series X controller, SteamInput Virtual Controller, and the Google Stadia Controller.

### Audio
Not supported and probably never will be. It was working before but after changing screens it would crash. I don't know why and I don't care enough to fix it.

### Assets
Blade uses a custom asset system. Assets are stored in the Assets folder.

### Leaderboard
Blade uses a custom leaderboard system. Leaderboard are stored as json file.
- Linux: ~/.config/Blade/\<GAME\>/leaderboard.json
- Windows: %APPDATA%\Blade\\<GAME\>\leaderboard.json

## Building
All native dependencies are included in the repo. The only thing you need to install is the .NET 7 SDK.

## History
Blade was orignily crated to run direcly on the terminal. It was later ported to use OpenTK for rendering. The original terminal version can be found in the `terminal` branch. The original version was drawing to fast for most terminals to handle. So this updated version was created to use OpenTK for rendering.