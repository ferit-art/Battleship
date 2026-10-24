namespace Battleship;

public abstract class Person
{
    public string Name { get; }
    public Board Board { get; }
    private List<Ship> _ships = new();
    
    protected Person(string name)
    {
        Name = name;
        Board = new Board(Name);
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

    public virtual void Hit(Coordinate target, Person targetPerson)
    {
        Person attacker = this;
        
        foreach (var ship in targetPerson._ships)
        {
            if (ship.ShipHit(target))
            {
                ship.TakeHit(target, attacker);   
            }
        }

        // Complicated
        targetPerson.Board.CoordinateLayout.First(c => c == target).ChangeStatus("Hit");
    }
}