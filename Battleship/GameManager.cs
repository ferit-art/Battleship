namespace Battleship;

public class GameManager
{
    public void StartGame()
    {
        Console.Clear();
        Game.s_player.Board.PrintVisual();
        Game.s_bot.Board.PrintVisual();
        
        //  TODO:  Let the player place their ships
    }
    
    public void PlayerTurn()
    {
        Thread.Sleep(1000);
    }

    public void BotTurn()
    {
        Thread.Sleep(1000);
    }
}