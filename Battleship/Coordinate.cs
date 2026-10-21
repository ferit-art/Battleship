namespace Battleship;

public class Coordinate
{
    public string Board { get; }
    private int XValue { get; }
    private int YValue { get; }
    public string Status { get; private set; }

    public Coordinate(int xValue, int yValue, string board)
    {
        XValue = xValue;
        YValue = yValue;
        Board = board;
        Status = "";
    }

    //  Maybe move the if comparisons to the Board.cs and make it even simpler in the Change methods.
    public void ChangeStatus(string statusNew)
    {
        Status = statusNew;
    }

    public string ToText()
    {
        return $"({XValue},{YValue})";
    }
}