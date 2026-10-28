namespace Battleship;

public class Bot() : Person("Bot")
{
    private readonly Random _random = new();
    
    public Coordinate Guess()
    {
        string letters = "ABCDEFGHIJ";
        int x = _random.Next(0, 10);
        int yInt = _random.Next(0, 10);
        char y = letters[yInt];

        return Game.s_players.OfType<Player>().FirstOrDefault().Board.GetCoordinate(x, y);
    }
}