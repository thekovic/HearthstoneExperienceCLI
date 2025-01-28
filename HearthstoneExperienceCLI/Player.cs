using static HearthstoneExperienceCLI.Globals;

namespace HearthstoneExperienceCLI;

public class Player(string name)
{
    public string Name { get { return name; } }

    public Deck? Deck { get; set; }

    public Rank Rank { get; set; } = Rank.Bronze10;

    public int Gold { get; set; } = 0;

    public bool HasWinStreak { get; set; } = false;

    public bool MustSkipTurn { get; set; } = false;

    public void ReceiveGold()
    {
        if (this.Deck == null)
        {
            return;
        }

        this.Gold += this.Deck.DeckType switch
        {
            DeckType.Tier1 => 1,
            DeckType.Tier2 => 2,
            DeckType.Tier3 => 3,
            DeckType.Tier4 => 4,
            DeckType.OffMeta => RNG.GenerateOffMetaDeckGold(),
            _ => throw new NotImplementedException()
        };
    }

    public bool BuyItem(Item item)
    {
        switch (item)
        {
            case Item.DeckSwap:
            case Item.WinStreak:
                if (this.Gold < 3)
                {
                    return false;
                }

                this.Gold -= 3;
                break;
            case Item.CardPatch:
                if (this.Gold < 4)
                {
                    return false;
                }

                this.Gold -= 4;
                break;
            case Item.ServerCrash:
                if (this.Gold < 6)
                {
                    return false;
                }

                this.Gold -= 6;
                break;
        }

        return true;
    }

    public void ProcessMatchResult(MatchResult result)
    {
        int currentRank = (int) this.Rank;

        switch (result)
        {
            case MatchResult.Win:
                if (this.HasWinStreak)
                {
                    currentRank += 2;
                    this.HasWinStreak = false;
                }
                else
                {
                    currentRank += 1;
                }

                Console.WriteLine("Match won!");
                break;
            case MatchResult.Loss:
                currentRank -= 1;
                this.HasWinStreak = false;
                Console.WriteLine("Match lost!");
                break;
            case MatchResult.Tie:
                Console.WriteLine("Match tied!");
                break;
        }

        if (currentRank < 0)
        {
            currentRank = 0;
        }

        if (currentRank > 11)
        {
            currentRank = 11;
        }

        this.Rank = (Rank) currentRank;
        if (result == MatchResult.Win)
        {
            Console.WriteLine($"{this.Name} ranks up to {this.Rank}!");
        }
        else if (result == MatchResult.Loss)
        {
            Console.WriteLine($"{this.Name} drops to {this.Rank}!");
        }
    }
}
