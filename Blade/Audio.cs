using LibVLCSharp.Shared;

namespace Blade;

public class Audio {
    private static LibVLC lib;
    public static Audio Instance { get; private set; }


    public static void Initialize() {
        Core.Initialize();
        lib = new();
        Instance = new Audio();
    }

    public MediaPlayer Play(string path) {
        var player = new MediaPlayer(lib);
        player.Play(new Media(lib, GetFile(path)));
        player.EndReached += (sender, e) => {
            player.Dispose();
        };
        return player;
    }

    private static string GetFile(string path) {
        var file = Assets.GetPath("Sounds", path);
        if (!File.Exists(file)) {
            throw new FileNotFoundException($"Audio file not found: {file}");
        }
        return file;
    }

    public MediaPlayer Play(string path, bool repeat) {
        var player = new MediaPlayer(lib);
        player.Play(new Media(lib, GetFile(path)));
        player.EndReached += (sender, e) => {
            if (repeat) {
                player.Position = 0;
                player.Play();
            }
        };
        return player;
    }

    public void Dispose() {
        lib.Dispose();
    }






}