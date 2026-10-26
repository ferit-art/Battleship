namespace Battleship;

public class Score
{
    public string Owner { get; }
    public int ShipCount { get; private set; }

    public Score(string person)
    {
        Owner = person;
        ShipCount = 5;
    }

    public void LowerCount(Person person)
    {
        if (person.Name == Owner)
        {
            ShipCount--;
        }
        else
        {
            Environment.Exit(0);
        }
    }
}