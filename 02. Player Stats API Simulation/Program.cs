namespace _02._Player_Stats_API_Simulation;

// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;

class PlayerStats
{

    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("sport")]
    public string Sport { get; set; }
    
    [JsonPropertyName("gamesPlayed")]
    public int GamesPlayed { get; set; }
    
    [JsonPropertyName("points")]
    public double Points { get; set; }

    public List<PlayerStats> ParsePlayerStats(string json)
    {
        List<PlayerStats> players = JsonSerializer.Deserialize<List<PlayerStats>>(json);

        return players;
    }

    public List<PlayerStats> GetTopScorers(List<PlayerStats> players, int topN)
    {
        List<PlayerStats> output = new List<PlayerStats>();
        int numberOfPlayersPrintedOut = 0;
        
        foreach (PlayerStats p in players.OrderByDescending(p => p.Points))
        {
            if (numberOfPlayersPrintedOut == topN)
            {
                break;
            }
            
            numberOfPlayersPrintedOut++;
            output.Add(p);
        }

        return output;
    }

    public Dictionary<string, double> AveragePointsBySport(List<PlayerStats> players)
    {
        Dictionary<string, double> output = new Dictionary<string, double>();
        
        foreach (PlayerStats player in players)
        {
            string sport = player.Sport;

            if (!output.ContainsKey(sport))
            {
                output.Add(sport, players.Where(p => p.Sport == sport).Average(p => p.Points));
            }
        }

        return output;
    }
}


class Program
{
    static void Main(string[] args)
    {
        PlayerStats ps = new PlayerStats();

        string json =
            """
            [ 
                { "name": "LeBron James", "sport": "Basketball", "gamesPlayed": 72, "points": 25.7 }, 
                { "name": "Patrick Mahomes", "sport": "Football", "gamesPlayed": 16, "points": 33.4 }, 
                { "name": "Lionel Messi", "sport": "Soccer", "gamesPlayed": 30, "points": 18.2 }, 
                { "name": "Nikola Jokic", "sport": "Basketball", "gamesPlayed": 79, "points": 26.4 } 
            ] 
            
            """;

        List<PlayerStats> players = ps.ParsePlayerStats(json);

        foreach (PlayerStats p in players)
        {
            Console.WriteLine(p.Name);
        }

        List<PlayerStats> topScorers = ps.GetTopScorers(players, 1); // Patrick Mahomes

        foreach (PlayerStats p in topScorers)
        {
            Console.WriteLine(p.Name);
        }

        Dictionary<string, double> averagePoints = ps.AveragePointsBySport(players);

        foreach (KeyValuePair<string, double> kvp in averagePoints)
        {
            Console.WriteLine($"{kvp.Key} -> Average: {kvp.Value:F2}");
        }
    }
}