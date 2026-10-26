namespace Battleship;

public abstract class Person
{
    public string Name { get; }
    public Board Board { get; }
    public List<Ship> _ships = new(); //    Change to private later.
    public Score Score { get; }
    
    protected Person(string name)
    {
        Name = name;
        Score = new Score(Name);
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
            if (ship.ShipHit(target)) //    If the target coordinate matches any of the ship coordinates.
            {
                ship.TakeHit(target, attacker);   
                Score.LowerCount(this);
            }
            else
            {
                // Complicated
                targetPerson.Board.CoordinateLayout.First(c => c == target).ChangeStatus("Miss");
            }
        }
    }
}