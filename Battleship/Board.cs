namespace Battleship;

public class Board
{
    public string Owner { get; }
    public List<Coordinate> CoordinateLayout { get; }

    public Board(string owner)
    {
        Owner = owner;

        CoordinateLayout = new List<Coordinate>();
        for (int i = 0; i <= 10; i++)
        {
            for (int j = 0; j <= 10; j++)
            {
                CoordinateLayout.Add(new Coordinate(j, i, this));
            }
        }
    }

    public void PrintVisual() //  Take it for what it is, TODO: "IF IT AIN'T BROKE, DON'T FIX IT"
    {
        Thread.Sleep(50);
        // "A " (2 chars) + 10 cells of "|__|" (40 chars) = 42
        int tableWidth = 42;

        // Ensure margin is never negative if terminal is smaller than 42 chars
        int leftMargin = Math.Max(0, (Console.WindowWidth - tableWidth) / 2);
        string indent = new string(' ', leftMargin);

        string letters = "ABCDEFGHIJ";

        for (int i = 0; i < CoordinateLayout.Count; i++)
        {
            Coordinate c = CoordinateLayout[i];

            // At the beginning of each row (X = 0), print the indentation
            if (c.XValue == 0)
            {
                Console.Write(indent);
                if (c.YValue == 0)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write($"{letters[CoordinateLayout[i - 1].YValue]} ");
                }
            }
            else
            {
                if (c.YValue == 0)
                {
                    if (c.XValue == 10)
                    {
                        Console.Write($"{" ",2}{i}");
                    }
                    else
                    {
                        Console.Write($"{" ",2}0{i}");
                    }
                }
                else
                {
                    Console.Write($"{c.ToText()}");
                }
            }

            if (c.XValue == 10)
            {
                Console.WriteLine();
            }
        }
    }
}