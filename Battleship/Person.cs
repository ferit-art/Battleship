namespace Battleship;

public class Person
{
    public string Name { get; }
    private List<Ship> _ships = new();
    
    protected Person(string name)
    {
        Name = name;
    }

    public void AddShip(Ship ship)
    {
        _ships.Add(ship);
    }

    public void Hit(Coordinate target)
    {
        target.ChangeStatus("Hit");
        Person targetPerson = null;
        Person attacker = null;
        
        foreach (var person in Game.s_persons)
        {
            if (person != this)
            {
                targetPerson = person;
            }
            else
            {
                attacker = person;
            }
        }
        
        foreach (var ship in targetPerson._ships)
        {
            ship.TakeHit(target, attacker);
        }
    }

    public void PlaceShip()
    {
        //  TODO: Loop through the Location<Coordinate> and change the Coordinate:s to the input values.
        
        
    }
}