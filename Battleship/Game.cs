namespace Battleship;

public class Game
{
    public GameManager GameManager = new();
    public static bool s_gameActive { get; set;}
    public static Bot s_bot { get; set;}
    public static Player s_player { get; set;}

    public Game()
    {
        InitializeGame();
        GameManager.WelcomeMessage();
        GameLoop();
    }

    private void InitializeGame()
    {
        s_gameActive = true;
        s_bot = new Bot();
        s_player = new Player();
    }

    public void GameLoop()
    {
        do
        {
            GameManager.StartGame();
            GameManager.PlayerTurn();
            
            if (s_gameActive)
            {
                GameManager.BotTurn();
            }

            if (GameManager.AskToPlayAgain())
            {
                GameManager.PlayAgain();
            }
            else
            {
                GameManager.Exit();
            }
        } while (s_gameActive);
    }
}