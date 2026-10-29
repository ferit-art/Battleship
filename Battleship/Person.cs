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
        if (targetPerson._ships.Any(s => s.ShipIsHit(target)))  //  If it is a HIT
        {
            Ship ship = targetPerson._ships.First(s => s.Location.Any(c => c.Equals(target)));
            ship.TakeHit(target, this);   
            Score.LowerCount(this);
        }
        else
        {
            targetPerson.Board.CoordinateLayout.First(c => c == target).ChangeStatus("Miss");
        }
    }
}