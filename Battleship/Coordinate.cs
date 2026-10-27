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
    
    public string ToText()
    {
        return Status switch
        {
            "Hit" => "| X|",
            "Miss" => "| O|",
            "Ship" => "| S|",
            _ => "|__|"
        };
    }
    
    public static bool CIsNextToAnotherC(Coordinate c1, Coordinate c2)
    {
        bool cIsUpOrDown = false;
        if (c1.YValue - 1 == c2.YValue || c1.YValue + 1 == c2.YValue)
        {
            if (c1.XValue == c2.XValue)
            {
                cIsUpOrDown = true;   
            }
        }

        bool cIsRightOrLeft = false;
        if (c1.XValue - 1 == c2.XValue || c1.XValue + 1 == c2.XValue)
        {
            if (c1.YValue == c2.YValue)
            {
                cIsRightOrLeft = true;   
            }
        }
        
        if (cIsRightOrLeft || cIsUpOrDown)
        {
            return true;   
        }

        return false;
    }
}