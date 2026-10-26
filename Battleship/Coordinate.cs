namespace Battleship;

public class Coordinate
{
    public Board Board { get; }
    public int XValue { get; }
    public int YValue { get; }
    public string Status { get; private set; }

    public Coordinate(int xValue, int yValue, Board board)
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
    
    private static readonly string Letters = "ABCDEFGHIJK"; //  For navigation purposes.

    public string ToText()
    {
        char colLetter = Letters[XValue];
        return "|__|";
    }
}