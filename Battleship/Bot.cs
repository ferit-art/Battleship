namespace Battleship;

public class Bot() : Person("Bot")
{
    private Random _random = new();
    
    public Coordinate Guess()
    {
        int x = _random.Next(0, 10);
        int y = _random.Next(0, 10);
        //  Might not work later
        return new Coordinate(x, y, Game.s_players.OfType<Player>().First().Board);
    }
}