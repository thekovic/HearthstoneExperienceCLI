using static HearthstoneExperienceCLI.Globals;

namespace HearthstoneExperienceCLI;

public class Match
{
    private int _winChance = 10;
    private int _lossChance = 10;
    private int _tieChance = 1;

    private double Total {  get {  return _winChance + _lossChance + _tieChance; } }

    public void ApplyModifier(int modifier)
    {
        if (modifier < 0)
        {
            _lossChance -= modifier;
        }
        else
        {
            _winChance += modifier;
        }
    }

    public MatchResult GetMatchResult()
    {
        double winChance = _winChance / this.Total;
        double tieChance = _tieChance / this.Total;
        double lossChance = _lossChance / this.Total;

        double lossThreshold = winChance + tieChance;

        double matchResult = RNG.GenerateMatchResult();

        Console.WriteLine($"  Win chance: {_winChance} ({(winChance * 100):F2}%)");
        Console.WriteLine($"  Loss chance: {_lossChance} ({(lossChance * 100):F2}%)");
        Console.ReadLine();
#if DEBUG
        Console.WriteLine($"Roll: {matchResult:F4} (Win <= {winChance:F4}; Loss >= {lossThreshold:F4})");
        
#endif
        if (matchResult <= winChance)
        {
            return MatchResult.Win;
        }
        else if (matchResult >= lossThreshold)
        {
            return MatchResult.Loss;
        }
        else
        {
            return MatchResult.Tie;
        }
    }

    public static int ProcessMatchModifier(Player currentPlayer, MatchModifier modifier)
    {
        switch (modifier)
        {
            case MatchModifier.BanFromBlizzard:
                Console.WriteLine("You got banned by Blizzard! You make a new alt but it means you're back in Bronze 10...");
                currentPlayer.Rank = Rank.Bronze10;
                return 0;

            case MatchModifier.GetCardPatchItem:
                Console.WriteLine("You receive a bonus Patch a Card item!");
                Game.ProcessItem(currentPlayer, Item.CardPatch);
                return 0;

            case MatchModifier.GetServerCrashItem:
                Console.WriteLine("You receive a bonus Server Crash item!");
                Game.ProcessItem(currentPlayer, Item.ServerCrash);
                return 0;

            case MatchModifier.BotOpponent:
                Console.WriteLine("Your opponent is a bot.");
                Console.WriteLine("Match modifier: +9");
                return 9;

            case MatchModifier.WatchedGuide:
                Console.WriteLine("You watched a guide from Kripp on how to play this deck.");
                Console.WriteLine("Match modifier: +2");
                return 2;

            case MatchModifier.Distracted:
                Console.WriteLine("You got a little distracted while playing and made a misplay.");
                Console.WriteLine("Match modifier: -1");
                return -1;

            case MatchModifier.OnTheToilet:
                Console.WriteLine("You went to the toilet while looking for a match and...");
                int toiletModifier = RNG.GenerateToiletModifier();
                if (toiletModifier < -6)
                {
                    Console.WriteLine("You had a really nasty shit.");
                }
                else if (toiletModifier < 0)
                {
                    Console.WriteLine("It was more than a three-wiper.");
                }
                else if (toiletModifier < 5)
                {
                    Console.WriteLine("You had a quick twinkle.");
                }
                else
                {
                    Console.WriteLine("You had an amazing shit.");
                }

                char sign = (toiletModifier > 0) ? '+' : ' ';
                Console.WriteLine($"Match modifier: {sign}{toiletModifier}");
                return toiletModifier;

            case MatchModifier.RopedOut:
                Console.WriteLine("You roped out.");
                Console.WriteLine("Match modifier: -2");
                return -2;

            case MatchModifier.GotTopDecked:
                Console.WriteLine("Your opponent got a lucky top deck.");
                Console.WriteLine("Match modifier: -4");
                return -4;

            case MatchModifier.TopDecked:
                Console.WriteLine("You got a lucky top deck.");
                Console.WriteLine("Match modifier: +4");
                return 4;

            case MatchModifier.BadMatchUp:
                Console.WriteLine("You got a bad matchup.");
                Console.WriteLine("Match modifier: -6");
                return -6;

            case MatchModifier.GoodMatchUp:
                Console.WriteLine("You got a good matchup.");
                Console.WriteLine("Match modifier: +6");
                return 6;

            case MatchModifier.GotCoaching:
                Console.WriteLine("You got professional coaching on your deck.");
                Console.WriteLine("Match modifier: +7");
                return 7;

            case MatchModifier.ChangeDeck:
                Console.WriteLine("You impulsively decide to switch up your deck.");
                currentPlayer.Deck = RNG.GenerateDeck();
                Console.WriteLine($"{currentPlayer.Name} built a {currentPlayer.Deck.DeckTypeString} deck (strength {currentPlayer.Deck.Strength}).");
                return 0;

            case MatchModifier.GetWinStreak:
                Console.WriteLine("You receive a bonus win streak!");
                currentPlayer.HasWinStreak = true;
                return 0;

            case MatchModifier.Disconnected:
                Console.WriteLine("Your internet failed and you disconnected during the match.");
                Console.WriteLine("Match modifier: -7");
                return -7;

            case MatchModifier.HorribleRNG:
                Console.WriteLine("You got HORRIBLE RNG!");
                Console.WriteLine("Match modifier: -5");
                return -5;

            case MatchModifier.InsaneRNG:
                Console.WriteLine("You got INSANE RNG!");
                Console.WriteLine("Match modifier: +5");
                return 5;

            case MatchModifier.DeckTracker:
                Console.WriteLine("You decided to install a deck tracker. It's super effective!");
                Console.WriteLine("Match modifier: +8");
                return 8;
            default:
                throw new NotImplementedException();
        }
    }
}
