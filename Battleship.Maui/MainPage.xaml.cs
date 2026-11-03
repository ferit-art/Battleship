namespace Battleship.Maui;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object? sender, EventArgs e)
    {
        count++;
        
        //  SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private void OnStartGameClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnExitGameClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnHowToPlayClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}