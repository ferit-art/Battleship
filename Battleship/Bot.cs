namespace Battleship;

public class Bot() : Person("Bot")
{
    private readonly Random _random = new();
    
    public Coordinate Guess(Board board)
    {
        string letters = "ABCDEFGHIJ";
        int x = _random.Next(1, 11);
        int yInt = _random.Next(0, 10);
        char y = letters[yInt];

        if (board.CoordinateLayout == Board.CoordinateLayout)
        {
            return Board.GetCoordinate(x, y);
        }
        return Game.s_players.OfType<Player>().First().Board.GetCoordinate(x, y);
    }

    public void PlaceShips()
    {
        int shipNo = _random.Next(1, 6);

        switch (shipNo)
        {
            case 1:
                AddShip(new Carrier(this, GetNewShipLocation(5)));
                break;
            case 2:
                AddShip(new Battleship(this, GetNewShipLocation(4)));
                break;
            case 3:
                AddShip(new Destroyer(this, GetNewShipLocation(3)));
                break;
            case 4:
                AddShip(new Submarine(this, GetNewShipLocation(3)));
                break;
            case 5:
                AddShip(new PatrolBoat(this, GetNewShipLocation(2)));
                break;
        }
    }

    public List<Coordinate> GetNewShipLocation(int lenght)
    {
        List<Coordinate> location = new();
        Coordinate lastC = null;
        for (int i = 0; i < lenght; i++)
        {
            Coordinate? c = null;
            while (c == null)   //  Guesses until it guesses in the right parameters
            {
                c = Guess(Board);
                if (lastC == null || Coordinate.CIsNextToAnotherC(lastC, c))
                {
                    if (c.ToText() == "|__|")
                    {
                        if (i > 1)
                        {
                            if (GameManager.LineIsStraight(location, c))
                            {
                                c.ChangeStatus("Ship");
                                location.Add(c);
                                lastC = c;   
                            }
                            else
                            {
                                c = null;
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
                        c = null;
                    }
                }
                else
                {
                    c = null;
                }
            }
        }

        return location;
    }
}