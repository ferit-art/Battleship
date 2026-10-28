namespace Battleship;

public class GameManager
{
    public void WelcomeMessage()
    {
        int listWidth = 25;
        int leftMargin = Math.Max(0, (Console.WindowWidth - listWidth) / 2);
        string indent = new string(' ', leftMargin);
        
        Console.Clear();
        Console.WriteLine($"{indent}Welcome to Battleship, rules are simple;");
        Console.WriteLine($"{indent}1.  Place your ships.");
        Console.WriteLine($"{indent}2.  Try to guess and sink opponent's ships.");
        Console.WriteLine("\n" + $"{indent}First come, first served. A good luck is needed.");
        Console.WriteLine("Press a key to continue:");
    }
    public void StartGame()
    {
        Console.Clear();
        Game.s_player.Board.PrintVisual();

        PrintShipsToPlace();
        PlaceShips();   //  Numbers first (like 1A)
        Game.s_player.Board.PrintVisual();
        Thread.Sleep(3000);
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
        List<Coordinate> location = new();
        Coordinate lastC = null;
        bool shipIsToBeStraight = false;
        
        for (int i = 0; i < length; i++)    //  Iterates through the amount of C
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
                Console.WriteLine($"Choose the {i+1}. coordinate:");
                answer = Console.ReadLine();

                try
                {
                    string xVal;
                    char yVal;
                    int yValInt;
                    
                    if (answer.Length == 3) //  Incase of X = 10
                    {
                        xVal = answer.Substring(0,2);
                        yVal = answer[2];
                        yValInt = letters.IndexOf(yVal);    
                    }
                    else
                    {
                        xVal = answer.Substring(0,1);
                        yVal = answer[1];
                        yValInt = letters.IndexOf(yVal);
                    }
                    int targetX = int.Parse(xVal);

                    if (yValInt != -1 && targetX > 0)
                    {
                        var c = Game.s_player.Board.GetCoordinate(targetX, yVal);
                        if (lastC == null || Coordinate.CIsNextToAnotherC(lastC, c))
                        {
                            if (shipIsToBeStraight)
                            {
                                if (LineIsStraight(location, c))
                                {
                                    c.ChangeStatus("Ship");
                                    location.Add(c);
                                    lastC = c;   
                                }
                                else   //   In case the coordinates don't make a straight line
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
                        else   //   Incase the coordinates aren't next to each other
                        {
                            throw new FormatException("Choose a coordinate next to the first one");
                        }
                    }
                    else   //  Incase the letter or x number doesn't match
                    {
                        throw new FormatException();
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

    private bool LineIsStraight(List<Coordinate> location, Coordinate c)
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
        Thread.Sleep(1000);
    }

    public void BotTurn()
    {
        Thread.Sleep(1000);
    }
}