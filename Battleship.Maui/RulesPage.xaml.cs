using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Maui;

public partial class RulesPage : ContentPage
{
    public RulesPage()
    {
        InitializeComponent();
    }

    private async void OnGoBackClicked(object? sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new MainPage());
        }
        catch (Exception x)
        {
            await DisplayAlert("DEBUG EXCEPTION", $"{x.Message}\n\nStack:\n{x.StackTrace}", "Dismiss");
        }
    }
}