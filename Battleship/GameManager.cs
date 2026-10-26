namespace Battleship;

public class GameManager
{
    public void StartGame()
    {
        Console.Clear();
        Game.s_player.Board.PrintVisual();

        PrintShipsToPlace();
        PlaceShips();   //  TODO: Test this piece of shit... Input A2 gave Invalid Input

        foreach (Ship ship in Game.s_player._ships)
        {
            foreach (var c in ship.Location)
            {
                Console.WriteLine($"{c.XValue}, {c.YValue}");
            }
        }
    }

    public void PrintShipsToPlace()
    {
        int listWidth = 25; // Length of "[4] Patrol Boat (2 boxes)"
        int leftMargin = Math.Max(0, (Console.WindowWidth - listWidth) / 2);
        string indent = new string(' ', leftMargin);

        Console.WriteLine();
        Console.WriteLine($"{indent}Available Ships to place:");
        Console.WriteLine($"{indent}[1] Carrier (5 boxes)");
        Console.WriteLine($"{indent}[2] Battleship (4 boxes)");
        Console.WriteLine($"{indent}[3] Destroyer (3 boxes)");
        Console.WriteLine($"{indent}[4] Submarine (3 boxes)");
        Console.WriteLine($"{indent}[5] Patrol Boat (2 boxes)");
    }

    public void PlaceShips()
    {
        string answer;
        
        do
        {
            answer = Console.ReadLine();
            List<Coordinate> shipLocation;

            switch (answer)
            {
                case "1":
                    shipLocation = AskShipCoordinates(5);
                    Game.s_player.AddShip(new Carrier(Game.s_player, shipLocation));
                    break;
                case "2":
                    shipLocation = AskShipCoordinates(4);
                    Game.s_player.AddShip(new Battleship(Game.s_player, shipLocation));
                    break;
                case "3":
                    shipLocation = AskShipCoordinates(3);
                    Game.s_player.AddShip(new Destroyer(Game.s_player, shipLocation));
                    break;
                case "4":
                    shipLocation = AskShipCoordinates(3);
                    Game.s_player.AddShip(new Submarine(Game.s_player, shipLocation));
                    break;
                case "5":
                    shipLocation = AskShipCoordinates(2);
                    Game.s_player.AddShip(new PatrolBoat(Game.s_player, shipLocation));
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
        } while (answer == null);
    }

    private List<Coordinate> AskShipCoordinates(int length)
    {
        string letters = "ABCDEFGHIJ";

        Console.Clear();
        Game.s_player.Board.PrintVisual();
        Console.WriteLine();

        List<Coordinate> location = new();
        for (int i = length; i > 0; i--)
        {
            //  Question loop
            string answer;
            do
            {
                Console.WriteLine($"Choose {i} more coordinate(s):");
                answer = Console.ReadLine();

                try
                {
                    string xVal = answer.Substring(0,1);
                    char yVal = answer[1];
                    Coordinate c = new Coordinate(int.Parse(xVal), letters.IndexOf(yVal), Game.s_player.Board);
                    location.Add(c);
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input.");
                    Thread.Sleep(2000);
                    answer = null;
                }
            } while (answer == null);
        }
        return location;
    }

    public void PlayerTurn()
    {
        Thread.Sleep(1000);
    }

    public void BotTurn()
    {
        Thread.Sleep(1000);
    }
}