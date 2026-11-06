namespace Battleship;

public abstract class Person
{
    public string Name { get; }
    public Board Board { get; }
    public List<Ship> Ships = new(); //    Change to private later.
    public Score Score { get; set; }
    public bool HasWon { get; set; }
    public bool HasAHit { get; set; }
    
    protected Person(string name)
    {
        Name = name;
        Score = new Score(Name);
        Board = new Board(Name);
        HasWon = false;
        HasAHit = false;
    }

    public void AddShip(Ship ship)
    {
        if (ship.Owner == this)
        {
            Ships.Add(ship);
        }
        else
        {
            Environment.Exit(0);
        }
    }

    public virtual void Hit(Coordinate target, Person targetPerson)
    {
        if (targetPerson.Ships.Any(s => s.ShipIsHit(target)))  //  If it is a HIT
        {
            Ship ship = targetPerson.Ships.First(s => s.Location.Any(c => c.Equals(target)));
            ship.TakeHit(target, this);   
            HasAHit = true;
        }
        else
        {
            HasAHit = false;
            targetPerson.Board.CoordinateLayout.First(c => c == target).ChangeStatus("Miss");
        }
    }
}