namespace Battleship;

public abstract class Ship
{
    public string Name { get; }
    public Person Owner { get; }
    public List<Coordinate> Location { get; }   //  Change to private later.
    public bool Sunken { get; private set; }
    
    protected Ship(string name, Person owner, List<Coordinate> location)
    {
        Name = name;
        Owner = owner;
        Location = location;
        Sunken = false;
    }

    public void Sink()
    {
        Sunken = true;
        Owner.Score.LowerCount(Owner);
    }

    public bool ShipIsHit(Coordinate target)
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
                Sink();
            }
        }
        else
        {
            Environment.Exit(0);
        }
    }
}