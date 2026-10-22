namespace Battleship;

public class Board
{
    public string Owner { get; }
    public List<Coordinate> CoordinateLayout { get; }

    public Board(string owner)
    {
        Owner = owner;

        CoordinateLayout= new List<Coordinate>();
        for (int i = 0; i <= 10; i++)
        {
            for (int j = 0; j <= 10; j++)
            {
                CoordinateLayout.Add(new Coordinate(i++, j++, this));   
            }
        }
    }
}