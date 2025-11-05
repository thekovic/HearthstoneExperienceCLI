using static HearthstoneExperienceCLI.Globals;

namespace HearthstoneExperienceCLI;

public class Deck(DeckType deckType, int strength)
{
    public DeckType DeckType { get; } = deckType;

    public int Strength { get; set; } = strength;

    public string DeckTypeString
    {
        get
        {
            var deckAsString = this.DeckType switch
            {
                DeckType.Tier1 => "Tier 1",
                DeckType.Tier2 => "Tier 2",
                DeckType.Tier3 => "Tier 3",
                DeckType.Tier4 => "Tier 4",
                DeckType.OffMeta => "Off-Meta",
                _ => throw new NotImplementedException()
            };

            return deckAsString;
        }
    }

    public int SellValue
    {
        get
        {
            return this.DeckType switch
            {
                DeckType.Tier1 => 1,
                DeckType.Tier2 => 2,
                DeckType.Tier3 => 3,
                DeckType.Tier4 => 4,
                DeckType.OffMeta => RNG.GenerateOffMetaDeckGold(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
