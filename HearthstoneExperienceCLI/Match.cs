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
        double lossChance = winChance + tieChance;

        double matchResult = RNG.GenerateMatchResult();

        Console.WriteLine($"  Win chance: {_winChance} ({winChance:F2}%)");
        double trueLossChance = _lossChance / this.Total;
        Console.WriteLine($"  Loss chance: {_lossChance} ({trueLossChance:F2}%)");
        Console.ReadLine();
#if DEBUG
        Console.WriteLine($"Roll: {matchResult:F2} (Win <= {winChance:F2}; Loss >= {lossChance:F2})");
        
#endif
        if (matchResult <= winChance)
        {
            return MatchResult.Win;
        }
        else if (matchResult >= lossChance)
        {
            return MatchResult.Loss;
        }
        else
        {
            return MatchResult.Tie;
        }
    }
}
