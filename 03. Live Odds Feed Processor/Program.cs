using System.Text.Json;
using System.Text.Json.Serialization;

namespace _03._Live_Odds_Feed_Processor;

class Odds
{
    [JsonPropertyName("sportsbook")]
    public string SportsBook { get; set; }
    
    [JsonPropertyName("homeOdds")]
    public double HomeOdds { get; set; }
    
    [JsonPropertyName("awayOdds")]
    public double AwayOdds { get; set;  }
    
}

class Event
{
    [JsonPropertyName("eventName")]
    public string EventName { get; set; }
    
    [JsonPropertyName("market")]
    public string Market { get; set; }

    [JsonPropertyName("odds")]
    public List<Odds> Odds { get; set; }
}

static class EventHandler
{
    public static List<Event> ParseFeed(string json)
    {
        return JsonSerializer.Deserialize<List<Event>>(json);
    }

    public static Odds GetBestHomeOdds(Event e)
    {
        Odds bestOdds = e.Odds.OrderByDescending(o => o.HomeOdds).ToList()[0];
        return bestOdds;
    }

    public static Odds GetBestAwayOdds(Event e)
    {
        Odds bestOdds = e.Odds.OrderByDescending(o => o.AwayOdds).ToList()[0];
        return bestOdds;
    }

    public static Dictionary<string, string> BestHomeOddsPerEvent(List<Event> events)
    {
        Dictionary<string, string> eventMap = new Dictionary<string, string>();
        
        foreach (Event e in events)
        {
            string name = e.EventName;
            
            if (!eventMap.ContainsKey(name))
            {
                string bestOddsBookName = GetBestHomeOdds(e).SportsBook;
                eventMap.Add(name, bestOddsBookName);
            }
        }

        return eventMap;
    }

    public static bool HasArbitrageOpportunity(Event e)
    {
        double bestHomeOdds = GetBestHomeOdds(e).HomeOdds;
        double bestAwayOdds = GetBestAwayOdds(e).AwayOdds;
        
        
        return ((1 / bestHomeOdds) + (1 / bestAwayOdds)) < 1;
    }
}

class Program
{
    static void Main(string[] args)
    {
        string json = """
                      [
                        {
                          "eventName": "Lakers vs Celtics",
                          "market": "Moneyline",
                          "odds": [
                            { "sportsbook": "DraftKings", "homeOdds": 1.85, "awayOdds": 2.05 },
                            { "sportsbook": "FanDuel", "homeOdds": 1.90, "awayOdds": 2.00 },
                            { "sportsbook": "BetMGM", "homeOdds": 1.80, "awayOdds": 2.10 }
                          ]
                        },
                        {
                          "eventName": "Chiefs vs Eagles",
                          "market": "Moneyline",
                          "odds": [
                            { "sportsbook": "DraftKings", "homeOdds": 1.70, "awayOdds": 2.25 },
                            { "sportsbook": "FanDuel", "homeOdds": 1.75, "awayOdds": 2.20 },
                            { "sportsbook": "BetMGM", "homeOdds": 1.65, "awayOdds": 2.30 }
                          ]
                        }
                      ]
                      """;
        
        List<Event> e = EventHandler.ParseFeed(json);
        Console.WriteLine(e[0].Odds[0].AwayOdds);
        
        Odds bestOdds = EventHandler.GetBestHomeOdds(e[1]);
        Console.WriteLine(bestOdds.SportsBook);

        Dictionary<string, string> eventsDict = EventHandler.BestHomeOddsPerEvent(e);
        Console.WriteLine(eventsDict["Lakers vs Celtics"]);

        foreach (Event ev in e)
        {
            Console.WriteLine(EventHandler.HasArbitrageOpportunity(ev));
        }

    }
}