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

    private static int ProcessMatchModifier(Player currentPlayer, MatchModifier modifier)
    {
        switch (modifier)
        {
            case MatchModifier.BanFromBlizzard:
                Console.WriteLine("You got banned by Blizzard! You make a new alt but it means you're back in Bronze 10...");
                currentPlayer.Rank = Rank.Bronze10;
                return 0;

            case MatchModifier.GetCardPatchItem:
                Console.WriteLine("You receive a bonus Patch a Card item!");
                ProcessItem(currentPlayer, Item.CardPatch);
                return 0;

            case MatchModifier.GetServerCrashItem:
                Console.WriteLine("You receive a bonus Server Crash item!");
                ProcessItem(currentPlayer, Item.ServerCrash);
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
                int matchModifier = ProcessMatchModifier(player, RNG.GenerateMatchModifier());
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
