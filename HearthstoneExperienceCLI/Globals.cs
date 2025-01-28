namespace HearthstoneExperienceCLI;

public class Globals
{
    public enum MatchResult
    {
        Win,
        Loss,
        Tie
    }

    public enum Rank
    {
        Bronze10,
        Bronze5,
        Silver10,
        Silver5,
        Gold10,
        Gold5,
        Platinum10,
        Platinum5,
        Diamond10,
        Diamond5,
        Diamond1,
        Legend
    }

    public enum DeckType
    {
        Tier1,
        Tier2,
        Tier3,
        Tier4,
        OffMeta
    }

    public enum Item
    {
        DeckSwap = 1,   // 3 gold
        WinStreak,      // 3 gold
        CardPatch,      // 4 gold
        ServerCrash     // 6 gold
    }

    public enum MatchModifier
    {
        BanFromBlizzard = 1,
        GetCardPatchItem,
        GetServerCrashItem,
        BotOpponent = 4,    // +9
        WatchedGuide = 10,  // +2
        Distracted = 16,    // -1
        OnTheToilet = 22,   // RNG (-9 to +10)
        RopedOut = 28,      // -2
        GotTopDecked = 34,  // -4
        TopDecked = 40,     // +4
        BadMatchUp = 46,    // -6
        GoodMatchUp = 52,   // +6
        GotCoaching = 58,   // +7
        ChangeDeck = 64,
        GetWinStreak = 70,
        Disconnected = 76,  // -7
        HorribleRNG = 82,   // -5
        InsaneRNG = 88,     // +5
        DeckTracker = 94    // +8
    }
}
