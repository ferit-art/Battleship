namespace Battleship.Maui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnStartGameClicked(object? sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new DeployPage());
        }
        catch (Exception x)
        {
            await DisplayAlert("DEBUG EXCEPTION", $"{x.Message}\n\nStack:\n{x.StackTrace}", "Dismiss");
        }
    }

    private async void OnExitGameClicked(object? sender, EventArgs e)
    {
        try
        {
            await DisplayAlert("EXIT MESSAGE", "YOU WANT TO EXIT? YEAH FINE GO AWAY THEN...", "DON'T CLICK", "DON'T CLICK");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            await DisplayAlert("DEBUG EXCEPTION", $"{ex.Message}\n\nStack:\n{ex.StackTrace}", "Dismiss");
        }
    }

    private async void OnHowToPlayClicked(object? sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new RulesPage());
        }
        catch (Exception x)
        {
            await DisplayAlert("DEBUG EXCEPTION", $"{x.Message}\n\nStack:\n{x.StackTrace}", "Dismiss");
        }
    }
}