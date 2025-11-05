using static HearthstoneExperienceCLI.Globals;

namespace HearthstoneExperienceCLI;

public class Shop
{
    public static void Visit(Player player)
    {
        if (player.Gold < 3)
        {
            Console.WriteLine($"{player.Name} can't afford anything in the shop (Current Gold: {player.Gold}).");
            return;
        }

        Console.WriteLine($"{player.Name} visits the shop (Current Gold: {player.Gold}):");
        while (true)
        {
            Console.WriteLine("  1. Swap decks with another player (3 Gold).");
            Console.WriteLine("  2. Activate a win streak -- if you win next game, you move up two ranks (3 Gold).");
            Console.WriteLine("  3. Buff/Nerf a card -- tier X deck gets +/-3 modifier until the next patch (4 Gold).");
            Console.WriteLine("  4. Server crash -- skip someone's turn (6 Gold).");
            Console.WriteLine("  0. Exit shop.");

        buyItemPrompt:
            Console.Write("Choose an item: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                continue;
            }

            int choice = int.Parse(input);
            if (choice == 0)
            {
                break;
            }

            var item = (Item) choice;
            var isValidPurchase = player.BuyItem(item);
            if (!isValidPurchase)
            {
                Console.WriteLine("Not enough gold!");
                goto buyItemPrompt;
            }
            else
            {
                Game.ProcessItem(player, item);
                break;
            }
        }
    }
}
