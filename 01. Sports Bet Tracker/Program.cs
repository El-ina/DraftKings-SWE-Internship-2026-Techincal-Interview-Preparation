namespace _01._Sports_Bet_Tracker;

// See https://aka.ms/new-console-template for more information

class Bet
{
    public int Id
    {
        get;
        set;
    }

    public string PlayerName
    {
        get;
        set;
    }
    
    public decimal Amount
    {
        get;
        set;
    }

    public double Odds
    {
        get;
        set;
    }
    
    public bool IsWon
    {
        get;
        set;
    }

    public Bet(int id, string playerName, decimal amount, double odds)
    {
        this.Id = id;
        this.PlayerName = playerName;
        this.Amount = amount;
        this.Odds = odds;
        this.IsWon = false;
    }

    public Bet(int id, string playerName, decimal amount, double odds, bool isWon) : this(id, playerName, amount, odds)
    {
        this.IsWon = isWon;
    }
}

class BetTracker
{
    private List<Bet> bets = new List<Bet>();

    public void AddBet(Bet bet)
    {
        this.bets.Add(bet);
    }

    public decimal CalculateBetPayout(Bet bet)
    {
        if (bet.IsWon)
        {
            return bet.Amount * (decimal)bet.Odds;
        }

        return 0;
    }
    
    public decimal SettleBet(int id, bool won)
    {
        Bet bet = bets.Where(b => b.Id == id).ToList()[0];
        bet.IsWon = won;

        return CalculateBetPayout(bet);
    }

    public decimal GetTotalPayout()
    {
        decimal sumPayouts = 0;
        
        foreach (Bet bet in bets)
        {
            sumPayouts += CalculateBetPayout(bet);
        }

        return sumPayouts;
    }

    public List<Bet> GetBetsByPlayer(string playerName)
    {
        List<Bet> betsForCurrPlayer = new List<Bet>();

        betsForCurrPlayer = bets.Where(b => b.PlayerName.ToLower() == playerName.ToLower()).ToList();

        return betsForCurrPlayer;
    }
}

class Program
{
    static void Main(string[] args)
    {
        BetTracker tracker = new BetTracker();

        tracker.AddBet(new Bet(1, "Alice", 50m, 2.5));
        tracker.AddBet(new Bet(2, "Bob", 100m, 1.8));
        tracker.AddBet(new Bet(3, "Alice", 30m, 3.0));

        Console.WriteLine("Settle bet 1 (won): " + tracker.SettleBet(1, true));   // 125
        Console.WriteLine("Settle bet 2 (lost): " + tracker.SettleBet(2, false)); // 0
        Console.WriteLine("Settle bet 3 (won): " + tracker.SettleBet(3, true));   // 90

        Console.WriteLine("Total payout: " + tracker.GetTotalPayout()); // 215

        List<Bet> aliceBets = tracker.GetBetsByPlayer("alice");
        Console.WriteLine("Alice's bets: " + aliceBets.Count); // 2
    }
}

