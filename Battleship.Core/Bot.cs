using System.Diagnostics;

namespace Battleship;

public class Bot() : Person("Bot")
{
    private readonly Random _random = new();
    public static string? GuessAxis;
    private int EnemyShipCount = 5;

    public Coordinate? Guess(Board board, int shipCount)
    {
        if (shipCount != EnemyShipCount)    //  Checks from bot memory
        {
            GuessAxis = null;
            EnemyShipCount = shipCount;
        }
        
        string letters = "ABCDEFGHIJ";
        int x = _random.Next(1, 11);
        int yInt = _random.Next(0, 10);
        char y = letters[yInt];

        Coordinate? c;
        if (board.CoordinateLayout == Board.CoordinateLayout)
        {
            c = Board.GetCoordinate(x, y);
            if (c.Status != "Hit" && c.Status != "Miss")
            {
                return c;
            }

            return null;
        }

        var possibleX = SearchX(board, shipCount); //   Searches after a previous hit and returns the c's available
                                                               //   that are nearest to the hit.
        if (possibleX != null)
        {
            return possibleX;
        }

        c = board.GetCoordinate(x, y);
        if (c.Status != "Hit" && c.Status != "Miss")
        {
            return c;
        }

        return null;
    }

    private Coordinate? SearchX(Board board, int shipCount)
    {
        List<Coordinate> hits = board.CoordinateLayout.Where(c =>
            c.ToText() == "|_X|").ToList();

        if (hits.Count != 0)
        {
            foreach (var x in hits)
            {
                SearchOrientation(board, x);   //    Checks if there are any hits
                                        //    near the C and sets the guess
                                        //    orientation

                List<Coordinate> cNearX;
                if (GuessAxis != null)
                {   //  Returns the available coords from the guess axis
                    List<Coordinate> pool;
                    if (GuessAxis == "y")
                    {
                        pool = board.CoordinateLayout.Where(c => c.XValue == x.XValue).ToList();
                    }
                    else
                    {
                        pool = board.CoordinateLayout.Where(c => c.YValue == x.YValue).ToList();
                    }
                    
                    cNearX = pool.Where(c => Coordinate.CIsNextToAnotherC(x, c) 
                                             && c.ToText() != "|_X|" && c.ToText() != "|_O|").ToList();
                }
                else
                {   //  If there aren't any previous hits near the last hit
                    cNearX = board.CoordinateLayout.Where(c =>
                        Coordinate.CIsNextToAnotherC(x, c) && c.ToText() != "|_X|"
                                                           && c.ToText() != "|_O|").ToList();
                }

                if (cNearX.Count != 0)
                {
                    return cNearX[0];
                }
            }
        }
        return null;
    }

    private void SearchOrientation(Board board, Coordinate? x)
    {
        if (GuessAxis == null)
        {
            var xNearX = board.CoordinateLayout.FirstOrDefault(c =>
                Coordinate.CIsNextToAnotherC(x, c) && c.ToText() == "|_X|");

            if (xNearX != null)
            {
                Debug.Assert(x != null, nameof(x) + " != null");
                GuessAxis = xNearX.XValue == x.XValue ? "y" : "x";
            }
        }
    }

    public void PlaceShips()
    {
        AddShip(new Carrier(this, GetNewShipLocation(5)));
        AddShip(new Battleship(this, GetNewShipLocation(4)));
        AddShip(new Destroyer(this, GetNewShipLocation(3)));
        AddShip(new Submarine(this, GetNewShipLocation(3)));
        AddShip(new PatrolBoat(this, GetNewShipLocation(2)));
    }

    public List<Coordinate> GetNewShipLocation(int length)
    {
        string letters = "ABCDEFGHIJ";
        List<Coordinate> location = new();
        bool placed = false;

        while (!placed)
        {
            location.Clear();

            bool isHorizontal = _random.Next(2) == 0;

            int startX;
            int startYIndex;

            if (isHorizontal)
            {
                // Ship grows horizontally along X, so X needs room to fit
                startX = _random.Next(1, 11 - length + 1);
                startYIndex = _random.Next(0, 10);
            }
            else
            {
                // Ship grows vertically along Y, so Y needs room to fit
                startX = _random.Next(1, 11);
                startYIndex = _random.Next(0, 10 - length + 1);
            }

            bool collision = false;
            for (int i = 0; i < length; i++)
            {
                int currentX;
                char currentLetter;

                if (isHorizontal)
                {
                    currentX = startX + i;
                    currentLetter = letters[startYIndex];
                }
                else
                {
                    currentX = startX;
                    currentLetter = letters[startYIndex + i];
                }

                Coordinate c = Board.GetCoordinate(currentX, currentLetter);
                if (c.Status != "")
                {
                    collision = true;
                    break;
                }

                location.Add(c);
            }

            if (!collision)
            {
                // All coordinates are clear
                foreach (var coord in location)
                {
                    coord.ChangeStatus("Ship");
                }

                placed = true;
            }
            // If there was a collision, loop repeats
        }

        return location;
    }
}