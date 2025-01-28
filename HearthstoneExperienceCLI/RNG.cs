using static HearthstoneExperienceCLI.Globals;

namespace HearthstoneExperienceCLI;

public class RNG
{
    private static Random _random = new();

    public static double GenerateMatchResult()
    {
        return _random.NextDouble();
    }

    public static Deck GenerateDeck()
    {
        var deckType = (DeckType) _random.Next(0, 5);
        int deckStrength = deckType switch
        {
            DeckType.Tier1 => 8,
            DeckType.Tier2 => 6,
            DeckType.Tier3 => 4,
            DeckType.Tier4 => 2,
            DeckType.OffMeta => GenerateOffMetaDeckStrength(),
            _ => throw new NotImplementedException()
        };

        return new Deck(deckType, deckStrength);
    }

    private static int GenerateOffMetaDeckStrength()
    {
        return _random.Next(0, 11);
    }

    public static int GenerateOffMetaDeckGold()
    {
        return _random.Next(0, 6);
    }

    public static MatchModifier GenerateMatchModifier()
    {
        int matchModifierInt = _random.Next(1, 100);

        var matchModifier = matchModifierInt switch
        {
            1 => MatchModifier.BanFromBlizzard,
            2 => MatchModifier.GetCardPatchItem,
            3 => MatchModifier.GetServerCrashItem,
            (>= 4) and (< 10) => MatchModifier.BotOpponent,
            (>= 10) and (< 16) => MatchModifier.WatchedGuide,
            (>= 16) and (< 22) => MatchModifier.Distracted,
            (>= 22) and (< 28) => MatchModifier.OnTheToilet,
            (>= 28) and (< 34) => MatchModifier.RopedOut,
            (>= 34) and (< 40) => MatchModifier.GotTopDecked,
            (>= 40) and (< 46) => MatchModifier.TopDecked,
            (>= 46) and (< 52) => MatchModifier.BadMatchUp,
            (>= 52) and (< 58) => MatchModifier.GoodMatchUp,
            (>= 58) and (< 64) => MatchModifier.GotCoaching,
            (>= 64) and (< 70) => MatchModifier.ChangeDeck,
            (>= 70) and (< 76) => MatchModifier.GetWinStreak,
            (>= 76) and (< 82) => MatchModifier.Disconnected,
            (>= 82) and (< 88) => MatchModifier.HorribleRNG,
            (>= 88) and (< 94) => MatchModifier.InsaneRNG,
            (>= 94) => MatchModifier.DeckTracker,
            _ => throw new NotImplementedException()
        };

        return matchModifier;
    }

    public static int GenerateToiletModifier()
    {
        return _random.Next(0, 20) - 9;
    }
}
