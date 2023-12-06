
using System.Text.Json;
using Blade;

namespace Blade;

public class Leaderboard {
    public List<KeyValuePair<string, int>> Scores { get; set; } = new();

    public string Name;

    public int HighScore => Scores.Count > 0 ? Scores[0].Value : 0;

    public Leaderboard(string name) {
        Name = name;
        Load();
    }

    public void AddScore(string name, int score) {
        Scores.Add(new KeyValuePair<string, int>(name, score));
        Scores = Scores.OrderByDescending(x => x.Value).ToList();
    }

    public void Save() {
        var file = Utils.GetAppFile(Name, "leaderboard.json");
        File.WriteAllText(file, JsonSerializer.Serialize(Scores));
    }

    public void Load() {
        var file = Utils.GetAppFile(Name, "leaderboard.json");
        if (File.Exists(file)) {
            Scores = JsonSerializer.Deserialize<List<KeyValuePair<string, int>>>(File.ReadAllText(file))!;
        }
    }

}

public class AddScoreMenu : Screen {
    private readonly Leaderboard leaderboard;

    public AddScoreMenu(Leaderboard leaderboard) {
        this.leaderboard = leaderboard;
    }



}

public class LeaderboardMenu : Screen {
    public ConsoleColor bgColor = ConsoleColor.Red;
    public ConsoleColor txtColor = ConsoleColor.White;
    public ConsoleColor fgColor = ConsoleColor.Black;
    public override int UpdateRate => 1000 / 10;

    const int MAX_NAME_LENGTH = 36;
    const int MAX_ENTRIES = 10;
    const int MAX_SCORE_LENGTH = 10;
    const int PADDING = 2;

    public Action Close { get; set; } = () => { };

    public override (int, int) Offset => GetCenter(Width, Height);

    int Height => Math.Clamp(Leaderboard.Scores.Count, 5, MAX_ENTRIES)
                 + MAX_SCORE_LENGTH + PADDING * 2;
    int Width = MAX_NAME_LENGTH + MAX_SCORE_LENGTH + (PADDING * 2);

    public required Leaderboard Leaderboard { get; set; }

    public override void Draw() {
        base.Draw();
        var size = MAX_NAME_LENGTH + MAX_SCORE_LENGTH;
        int line = 0;

        Paint(0, line, new string('▄', Width), bgColor);
        line++;
        Paint(0, line, Utils.CenterText("Leaderboard", Width), ConsoleColor.Black, bgColor);
        line++;
        Paint(0, line, "██" + new string('▀', size) + "██", bgColor, fgColor);

        for (int i = 0; i < MAX_ENTRIES; i++) {
            line++;
            Paint(0, line, new(' ', PADDING), txtColor, bgColor);
            if (i < Leaderboard.Scores.Count) {
                var score = Leaderboard.Scores[i];
                var str = $" {score.Key,-(MAX_NAME_LENGTH - 1)}";
                Paint(PADDING, line, str, txtColor, fgColor);
                str = Utils.RightText(score.Value.ToString() + " ", MAX_SCORE_LENGTH);
                Paint(PADDING + MAX_NAME_LENGTH, line, str, txtColor, fgColor);
            } else {
                Paint(PADDING, line, new(' ', size), txtColor, fgColor);
            }
            Paint(PADDING + size, line, new(' ', PADDING), txtColor, bgColor);

        }

        line++;
        Paint(0, line, "██" + new string('▄', size) + "██", bgColor, fgColor);
        line++;
        Paint(0, line, new string('█', Width), bgColor);
        Paint((Width / 2) - 5, line, Utils.CenterText("Close", 10), fgColor, txtColor);
        line++;
        Paint(0, line, new string('▀', Width), bgColor);
    }

    public override void OnKeyPress(ConsoleKeyInfo key) {
        base.OnKeyPress(key);
        if (key.Key == ConsoleKey.Escape) {
            Close();
        } else if (key.Key == ConsoleKey.Enter) {
            Close();
        }
    }

}