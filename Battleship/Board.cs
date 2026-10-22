namespace Battleship;

public class Board
{
    public string Color { get; }
    public List<Coordinate> CoordinateLayout { get; }

    public Board(string color)
    {
        Color = color;

        CoordinateLayout= new List<Coordinate>();
        for (int i = 0; i <= 10; i++)
        {
            for (int j = 0; j <= 10; j++)
            {
                CoordinateLayout.Add(new Coordinate(i++, j++, Color));   
            }
        }
    }
}