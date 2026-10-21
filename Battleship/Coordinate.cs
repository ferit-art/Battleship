namespace Battleship;

public class Coordinate
{
    public string FromBoard { get; }
    private char Axis { get; }
    private int Value { get; }
    public string Status { get; private set; }

    public Coordinate(char axis, int value, string board)
    {
        Axis = axis;
        Value = value;
        FromBoard = board;
        Status = "";
    }

    public void ChangeToHit(Person person)
    {
        if (person.HitBoard == FromBoard)
        {
            Status = "Hit";
        }
    }
    
    public void ChangeToMiss(Person person)
    {
        if (person.HitBoard == FromBoard)
        {
            Status = "Miss";
        }
    }

    public string ToText()
    {
        return $"{Axis}{Value}";
    }
}