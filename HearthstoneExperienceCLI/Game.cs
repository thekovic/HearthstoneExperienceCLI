using static HearthstoneExperienceCLI.Globals;

namespace HearthstoneExperienceCLI;

public class Game
{
    private static List<Player> _players = [];

    private static int _daysUntilPatch = 0;

    private static void PrintListOfPlayers()
    {
        int i = 1;
        foreach (var player in _players)
        {
            Console.WriteLine($"  {i}. {player.Name}");
            i++;
        }
    }

    public static void ProcessItem(Player currentPlayer, Item item)
    {
        switch (item)
        {
            case Item.DeckSwap:
            {
                Console.WriteLine("Swap a deck with another player.");
                PrintListOfPlayers();
                Console.Write("Choose a player: ");
                int choice = int.Parse(Console.ReadLine()!) - 1;
                var anotherPlayer = _players[choice];
                (anotherPlayer.Deck, currentPlayer.Deck) = (currentPlayer.Deck, anotherPlayer.Deck);
                break;
            }
            case Item.WinStreak:
            {
                Console.WriteLine("Win streak activated!");
                currentPlayer.HasWinStreak = true;
                break;
            }
            case Item.CardPatch:
            {
                Console.WriteLine("  1. Tier 1");
                Console.WriteLine("  2. Tier 2");
                Console.WriteLine("  3. Tier 3");
                Console.WriteLine("  4. Tier 4");
                Console.Write("Choose a tier: ");
                var tierChoice = (DeckType) int.Parse(Console.ReadLine()!) - 1;
                Console.WriteLine("  1. Buff (Modifier +3)");
                Console.WriteLine("  2. Nerf (Modifier -3)");
                Console.Write("Buff or Nerf: ");
                int modifier = (int.Parse(Console.ReadLine()!) == 1) ? 3 : -3;
                foreach (var player in _players)
                {
                    if (player.Deck!.DeckType == tierChoice)
                    {
                        player.Deck.Strength += modifier;
                    }
                }

                break;
            }
            case Item.ServerCrash:
            {
                Console.WriteLine("Crash the server to skip another player's turn.");
                PrintListOfPlayers();
                Console.Write("Choose a player: ");
                int choice = int.Parse(Console.ReadLine()!) - 1;
                _players[choice].MustSkipTurn = true;
                break;
            }
        }
    }

    private static bool CheckWinCondition()
    {
        foreach (var player in _players)
        {
            if (player.Rank == Rank.Legend)
            {
                Console.WriteLine($"{player.Name} achieved the rank of Legend and wins Hearthstone!");
                return true;
            }
        }

        return false;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to The Hearthstone Experience!");
        Console.WriteLine("Inspired by Rarran, Made by the_kovic, 2025");
        Console.WriteLine();

        Console.Write("Enter number of players: ");
        int numberOfPlayers = int.Parse(Console.ReadLine()!);
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Console.Write($"Enter name for Player {i + 1}: ");
            string playerName = Console.ReadLine()!;
            _players.Add(new Player(playerName));
        }

        Console.WriteLine();

        while (true)
        {
            if (_daysUntilPatch <= 0)
            {
                Console.WriteLine("New patch has released!");
                foreach (var player in _players)
                {
                    player.ReceiveGold();
                    player.Deck = RNG.GenerateDeck();
                    Console.WriteLine($"{player.Name} built a {player.Deck.DeckTypeString} deck (strength {player.Deck.Strength}).");
                }

                Console.WriteLine();

                foreach (var player in _players)
                {
                    Shop.Visit(player);
                }

                Console.WriteLine();
                _daysUntilPatch = 5;
            }

            Console.WriteLine($"{_daysUntilPatch} days left until the next patch comes out...");
            Console.WriteLine();

            foreach (var player in _players)
            {
                Console.WriteLine($"{player.Name} queues for Ranked in {player.Rank}...");
                if (player.MustSkipTurn)
                {
                    Console.WriteLine("Server crashed! Too bad.");
                    Console.WriteLine();
                    player.MustSkipTurn = false;
                    continue;
                }

                Match match = new();
                Console.WriteLine($"Deck strength: {player.Deck!.Strength}");
                int matchModifier = Match.ProcessMatchModifier(player, RNG.GenerateMatchModifier());
                match.ApplyModifier(matchModifier);
                match.ApplyModifier(player.Deck!.Strength);
                player.ProcessMatchResult(match.GetMatchResult());
                Console.ReadLine();
            }

            if (CheckWinCondition())
            {
                break;
            }

            _daysUntilPatch -= 1;
        }

        Console.WriteLine("Game over.");
    }
}
