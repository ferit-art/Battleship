namespace Battleship;

//  TODO:   Add Axis; that will determine in what type of a straight line the ship is gonna be, as
//  TODO:   the X or Y axis?
public abstract class Ship
{
    public Person Owner { get; }
    private int Lenght { get; }
    public List<Coordinate> Location { get; }   //  Change to private later.
    public bool Sunken { get; private set; }
    
    protected Ship(Person owner, int lenght, List<Coordinate> location)
    {
        Owner = owner;
        Lenght = lenght;
        Location = location;
        Sunken = false;
    }

    public void Sink(Person attacker)
    {
        Sunken = true;
        Owner.Score.LowerCount(Owner);
    }

    public bool ShipHit(Coordinate target)
    {
        if (Location.Any(c => c == target))
        {
            return true;
        }
        return false;
    }

    public void TakeHit(Coordinate target, Person attacker)
    {
        if (attacker != Owner)
        {
            int targetIndex = Location.IndexOf(target);
            Location[targetIndex].ChangeStatus("Hit");
            Location.Remove(target);

            bool sunken = false;
            foreach (var c in Location)
            {
                if (c.Status == "Hit")
                {
                    sunken = true;
                }
            }

            if (sunken)
            {
                Sink(attacker);
            }
        }
        else
        {
            Environment.Exit(0);
        }
    }
}