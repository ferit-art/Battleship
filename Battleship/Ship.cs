namespace Battleship;

public class Ship
{
    public Person Owner { get; }
    private int Lenght { get; }
    private List<Coordinate> Location { get;  }
    public bool Sunken { get; private set; }

    public Ship(Person owner, int lenght, List<Coordinate> location)
    {
        Owner = owner;
        Lenght = lenght;
        Location = location;
        Sunken = false;
    }
    
    public void Sink(Person attacker)
    {
        Sunken = true;
    }

    public void TakeHit(Coordinate target, Person attacker)
    {
        if (attacker != Owner)
        {
            int targetIndex = Location.IndexOf(target);
            Location[targetIndex].ChangeStatus("Hit");
            Location.Remove(target);
        }
    }
}