namespace Battleship;

public abstract class Person
{
    public string Name { get; }
    private List<Ship> _ships = new();
    
    protected Person(string name)
    {
        Name = name;
    }

    public void AddShip(Ship ship)
    {
        if (ship.Owner == this)
        {
            _ships.Add(ship);   
        }
        else
        {
            Environment.Exit(0);
        }
    }

    public void Hit(Coordinate target, Person targetPerson)
    {
        Person attacker = this;
        
        foreach (var ship in targetPerson._ships)
        {
            if (ship.ShipHit(target))
            {
                ship.TakeHit(target, attacker);   
            }
        }
    }
}