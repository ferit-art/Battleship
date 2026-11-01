namespace Battleship;

public class GameManager
{
    public static bool AllShipsArePlaced = false;

    public static void WelcomeMessage()
    {
        int listWidth = 25;
        int leftMargin = Math.Max(0, (Console.WindowWidth - listWidth) / 2);
        string indent = new string(' ', leftMargin);

        Console.Clear();
        Console.WriteLine($"{indent}Welcome to Battleship, rules are simple;");
        Console.WriteLine($"{indent}1.  Place your ships.");
        Console.WriteLine($"{indent}2.  Try to guess and sink opponent's ships.");
        Console.WriteLine("\n" + $"{indent}First come, first served. A good luck is indeed needed.");
        Thread.Sleep(5000);
        //  TODO: PRESS ANY KEY.
    }

    public void StartGame()
    {
        Console.Clear();
        Game.s_bot.PlaceShips();
        Game.s_player.Board.PrintVisual();

        do
        {
            PrintShipsToPlace();
            if (AllShipsArePlaced)
            {
                break;
            }
            PlaceShips(); //  Numbers first (like 1A)
        } while (!AllShipsArePlaced);

        Console.Clear();
        Game.s_player.Board.PrintVisual();
        Thread.Sleep(2500);
    }

    public static void PrintShipsToPlace()
    {
        string[] shipsToPlace = ["Carrier", "Battleship", "Destroyer", "Submarine", "Patrol Boat"];
        int listWidth = 25; // Length of "[4] Patrol Boat (2 boxes)"
        int leftMargin = Math.Max(0, (Console.WindowWidth - listWidth) / 2);
        string indent = new string(' ', leftMargin);

        Console.WriteLine();
        Console.WriteLine($"{indent}Available Ships to place:");

        if (Game.s_player._ships.Count != 0)
        {
            foreach (var shipToPlace in shipsToPlace)
            {
                if (!Game.s_player._ships.Any(s => s.Name == shipToPlace))
                {
                    Console.WriteLine($"{indent}[{shipsToPlace.IndexOf(shipToPlace) + 1}] " +
                                      $"{shipToPlace}");
                }
            }

            if (Game.s_player._ships.Count == 5)
            {
                AllShipsArePlaced = true;
                Console.WriteLine($"{indent}None left");
                // Console.Write("\nPress a key to continue:");
                //  Temporarily
                Thread.Sleep(2000);
                //  TODO: PRESS ANY KEY IMPLEMENTATION.
            }
        }
        else //   For the first time
        {
            Console.WriteLine($"{indent}[1] Carrier");
            Console.WriteLine($"{indent}[2] Battleship");
            Console.WriteLine($"{indent}[3] Destroyer");
            Console.WriteLine($"{indent}[4] Submarine");
            Console.WriteLine($"{indent}[5] Patrol Boat");
        }
    }

    public void PlaceShips()
    {
        string? answer;

        do
        {
            answer = Console.ReadLine();
            List<Coordinate> shipLocation;

            switch (answer)
            {
                case "1":
                    if (ChoiceIsNotPlaced(Game.s_player, "Carrier"))
                    {
                        shipLocation = AskShipCoordinates(5);
                        Game.s_player.AddShip(new Carrier(Game.s_player, shipLocation));
                    }
                    break;
                case "2":
                    if (ChoiceIsNotPlaced(Game.s_player, "Battleship"))
                    {
                        shipLocation = AskShipCoordinates(4);
                        Game.s_player.AddShip(new Battleship(Game.s_player, shipLocation));    
                    }
                    break;
                case "3":
                    if (ChoiceIsNotPlaced(Game.s_player, "Destroyer"))
                    {
                        shipLocation = AskShipCoordinates(3);
                        Game.s_player.AddShip(new Destroyer(Game.s_player, shipLocation));   
                    }
                    break;
                case "4":
                    if (ChoiceIsNotPlaced(Game.s_player, "Submarine"))
                    {
                        shipLocation = AskShipCoordinates(3);
                        Game.s_player.AddShip(new Submarine(Game.s_player, shipLocation));   
                    }
                    break;
                case "5":
                    if (ChoiceIsNotPlaced(Game.s_player, "Patrol Boat"))
                    {
                     
                        shipLocation = AskShipCoordinates(2);
                        Game.s_player.AddShip(new PatrolBoat(Game.s_player, shipLocation));   
                    }
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
        } while (answer == null);
    }

    public static bool ChoiceIsNotPlaced(Person person, string ship)
    {
        if (person._ships.Any(s => s.Name == ship))
        {
            return false;
        }

        return true;
    }

    private List<Coordinate> AskShipCoordinates(int length) //  TODO:   CHECK IF THE CHOSEN LENGHT FITS
        //  TODO:   THE REST OF THE C's IN THE DIRE
        //  TODO:   CTION OF THE 1. AND 2. C. OTHERWISE
        //  TODO:   THE USER IS SOFTLOCKED.
    {
        string letters = "ABCDEFGHIJ";
        List<Coordinate> location = new();
        Coordinate lastC = null;
        bool shipIsToBeStraight;

        for (int i = 0; i < length; i++) //  Iterates through the amount of C to be placed
            //  in ship type
        {
            if (i > 1)
            {
                shipIsToBeStraight = true;
            }
            else
            {
                shipIsToBeStraight = false;
            }

            //  Question loop
            string answer;
            do
            {
                Console.Clear();
                Game.s_player.Board.PrintVisual();
                Console.WriteLine();
                Console.WriteLine($"Choose the {i + 1}. coordinate:");
                answer = Console.ReadLine();

                try
                {
                    string xVal;
                    char yVal;

                    if (answer.Length == 3) //  Incase of X = 10
                    {
                        xVal = answer.Substring(0, 2);
                        yVal = answer[2];
                    }
                    else
                    {
                        xVal = answer.Substring(0, 1);
                        yVal = answer[1];
                    }

                    var c = Game.s_player.Board.GetCoordinate(int.Parse(xVal), yVal);
                    if (lastC == null || Coordinate.CIsNextToAnotherC(lastC, c))
                    {
                        if (c.ToText() == "|__|") //  If the c is not occupied of a status
                        {
                            if (shipIsToBeStraight)
                            {
                                if (LineIsStraight(location, c))
                                {
                                    c.ChangeStatus("Ship");
                                    location.Add(c);
                                    lastC = c;
                                }
                                else //   In case the coordinates don't make a straight line
                                {
                                    throw new FormatException("The coordinates must line up to make " +
                                                              "a straight ship");
                                }
                            }
                            else
                            {
                                c.ChangeStatus("Ship");
                                location.Add(c);
                                lastC = c;
                            }
                        }
                        else
                        {
                            throw new FormatException("The coordinate chosen must be unoccupied");
                        }
                    }
                    else //   Incase the coordinates aren't next to each other
                    {
                        throw new FormatException("Choose a coordinate next to the first one");
                    }
                }
                catch (Exception e)
                {
                    if (e.Message != null)
                    {
                        Console.WriteLine();
                        Console.WriteLine(e.Message);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid input. A correct example would be '1A'");
                    }

                    Thread.Sleep(2500);
                    answer = null;
                }
            } while (answer == null);
        }

        return location;
    }

    public static bool LineIsStraight(List<Coordinate> location, Coordinate c)
    {
        if (location[0].YValue == location[1].YValue)
        {
            return c.YValue == location[0].YValue;
        }

        if (location[0].XValue == location[1].XValue)
        {
            return c.XValue == location[0].XValue;
        }

        return false;
    }

    public void PlayerTurn()
    {
        do
        {
            int width = 25;
            int margin = Math.Max(0, (Console.WindowWidth - width) / 2);
            string indent = new string(' ', margin);

            Console.Clear();
            Console.WriteLine($"{indent}The computer's board:");
            Game.s_bot.Board.PrintVisual();
            Console.WriteLine();
            Console.WriteLine($"{indent}The player's board:");
            Game.s_player.Board.PrintVisual();

            AskTargetCoordinate();
            Thread.Sleep(1000);
            WinCheck(Game.s_player, Game.s_bot);
        } while (Game.s_player.HasAHit);
    }

    private void AskTargetCoordinate()
    {
        Console.WriteLine();

        string? answer;
        do
        {
            Console.Write("Choose a coordinate to strike:");
            answer = Console.ReadLine();
            string? message = null;

            try
            {
                string xVal;
                char yVal;

                if (answer!.Length == 3) //  Incase of X = 10
                {
                    xVal = answer.Substring(0, 2);
                    yVal = answer[2];
                }
                else
                {
                    xVal = answer.Substring(0, 1);
                    yVal = answer[1];
                }

                var c = Game.s_bot.Board.GetCoordinate(int.Parse(xVal), yVal);
                if (c.ToText() != "|__|" && c.ToText() != "|_S|")
                {
                    message = "Choose an unoccupied target";
                    throw new FormatException(message);
                }

                Game.s_player.Hit(c, Game.s_bot);

                Console.Clear();
                Game.s_bot.Board.PrintVisual();
                Console.WriteLine($"It's a [{c.Status}]");
            }
            catch (Exception e)
            {
                Console.WriteLine();

                if (e.Message == message)
                {
                    Console.WriteLine(e.Message);
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }

                Thread.Sleep(2000);
            }
        } while (answer == null);
    }

    public void BotTurn()
    {
        do
        {
            Coordinate? target;
            do
            {
                target = Game.s_bot.Guess(Game.s_player.Board);
            } while (target == null);

            Game.s_bot.Hit(target, Game.s_player);
            WinCheck(Game.s_bot, Game.s_player);

        } while (Game.s_bot.HasAHit && Game.s_gameActive);

        Thread.Sleep(1000);
    }

    public void WinCheck(Person attacker, Person target)
    {
        Person? winner;
        if (target._ships.All(s => s.Sunken))
        {
            attacker.HasWon = true;
            winner = attacker;
        }
        else
        {
            winner = null;
        }
        
        if (winner != null)
        {
            Thread.Sleep(1000);
            Console.WriteLine();
            Console.WriteLine($"The {winner.Name} has won this round.");
            Game.s_gameActive = false;
            attacker.HasAHit = false;
        }
    }

    public void Exit()
    {
        Console.Clear();
        Console.WriteLine("Exiting...");
        Thread.Sleep(2000);
        Environment.Exit(0);
    }

    public void PlayAgain()
    {
        Game.s_gameActive = true;
        AllShipsArePlaced = false;
        List<Person> players = [Game.s_bot, Game.s_player];

        foreach (var person in players)
        {
            foreach (var c in person.Board.CoordinateLayout)
            {
                c.ChangeStatus("");
            }
            person.HasWon = false;
            person.Score.ShipCount = 5;
            person._ships.Clear();
        }
    }

    public bool AskToPlayAgain()
    {
        string? answer;

        do
        {
            Console.Clear();
            Console.Write("One more round? [Y] or [N]: "); 
            answer = Console.ReadLine();
            
            switch (answer?.Trim().ToUpper())
            {
                case "Y":
                    return true;
                case "N":
                    return false;
                default:
                    Console.WriteLine("Invalid input.");
                    answer = null;
                    Thread.Sleep(2000);
                    break;
            }
        } while (answer == null);

        throw new InvalidOperationException();
    }
}