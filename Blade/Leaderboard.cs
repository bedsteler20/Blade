using System.Text.Json;

namespace Blade;


public class Leaderboard {
    public string Name { get; set; }
    public Dictionary<string, int> Scores { get; set; } = new Dictionary<string, int>();

    public Dictionary<string, string> GetData() {
        var sorted = Scores.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        var data = new Dictionary<string, string>();
        var i = 0;
        foreach (var score in sorted) {
            data.Add((i + 1).ToString(), $"{score.Key} - {score.Value}");
            i++;
        }
        return data;
    }

    public Leaderboard(string name) {
        Name = name;
        Load();
    }

    public void AddScore(string name, int score) {
        Scores.Add(name, score);
        Save();
    }


    public void Save() {
        var file = Utils.GetAppFile(Name, "leaderboard.json");
        File.WriteAllText(file, JsonSerializer.Serialize(Scores));
    }

    public void Load() {
        var file = Utils.GetAppFile(Name, "leaderboard.json");
        if (File.Exists(file)) {
            Scores = JsonSerializer.Deserialize<Dictionary<string, int>>(File.ReadAllText(file))!;
        }
    }
}

