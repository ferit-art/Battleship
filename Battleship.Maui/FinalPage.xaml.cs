using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Maui;

public partial class FinalPage : ContentPage
{
    private Player _player;
    private Bot _bot;
    private bool _gameActive;
    public FinalPage(Player player, Bot bot)
    {
        _player = player;
        _bot = bot;
        _gameActive = true;
        InitializeComponent();
        BuildBotGrid();
        BuildPlayerGrid();
    }

    private void BuildPlayerGrid()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerGrid.RowDefinitions.Add(new RowDefinition{ Height = GridLength.Star});    //  Star = Equal parts
            PlayerGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width = GridLength.Star});
        }
        for (int col = 0; col < 10; col++)
        {
            for (int row = 0; row < 10; row++)
            {
                var cellButton = new Button
                {
                    BackgroundColor = Color.FromArgb("#0A192F"),
                    BorderColor = Color.FromArgb("#1E3A5F"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    Padding = 0,
                    Margin = 1,
                    InputTransparent = true,

                    // Coordinates on the button  ||  (char)('A' + r) converts the r index to letter by summing it up with A's index
                    BindingContext = _player.Board.GetCoordinate(col + 1, (char)('A' + row))
                };

                if (_player.Board.GetCoordinate(col + 1, (char)('A' + row))?.ToText() == "|_S|")
                {
                    cellButton.Text = "S";
                    cellButton.FontAttributes = FontAttributes.Bold;
                }
                
                //  Places the cellbtn into the visual row and column
                Grid.SetRow(cellButton, row);
                Grid.SetColumn(cellButton, col);

                PlayerGrid.Children.Add(cellButton);
            }
        }
    }

    private void OnCellClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext != null)
        {
            dynamic c = btn.BindingContext;
            
            _player.Hit(c, _bot);
            
            if (_player.HasAHit)
            {
                btn.Text = "X";
                btn.FontAttributes = FontAttributes.Bold;
                
                if (_bot.Ships.Any( s => s.Location.Any(x => x == c) && s.Sunken))
                {
                    ShipHit(c, _bot);
                }
                else
                {
                    btn.TextColor = Colors.Red;
                    btn.BorderColor = Colors.Red;
                }
            }
            else
            {
                btn.Text = "O";
                btn.TextColor = Colors.White;
                btn.FontAttributes = FontAttributes.Bold;
            }
            
            WinCheck(_player, _bot);
            
            if (!_player.HasAHit && _gameActive)
            {
                BotAttacks();   
            }
            else
            {
                if (!_gameActive)
                {
                    foreach (var b in BotGrid.Children.OfType<Button>())
                    {
                        b.InputTransparent = true;
                    }
                }
            }
        }
    }

    private async void BotAttacks()
    {
        //  Disables the buttons for the player in the meanwhile
        foreach (var btn in BotGrid.Children.OfType<Button>())
        {
            btn.InputTransparent = true;
        }
        
        Random rand = new();
        
        do
        {
            int timeInSec = rand.Next(1, 3);
            await Task.Delay(timeInSec * 1000);

            Coordinate? target;
            do
            {
                target = _bot.Guess(_player.Board, _player.Score.ShipCount);
            } while (target == null);

            _bot.Hit(target, _player);
            Button btn = PlayerGrid.Children.OfType<Button>().First(b => b.BindingContext == target);
            
            if (_bot.HasAHit)
            {
                btn.Text = "X";
                btn.FontAttributes = FontAttributes.Bold;
                
                if (_player.Ships.Any( s => s.Location.Any(x => x == target) && s.Sunken))
                {
                    ShipHit(target, _player);
                }
                else
                {
                    btn.TextColor = Colors.Red;
                    btn.BorderColor = Colors.Red;
                }
            }
            else
            {
                btn.Text = "O";
                btn.TextColor = Colors.White;
                btn.FontAttributes = FontAttributes.Bold;
            }
            WinCheck(_bot, _player);
        } while (_bot.HasAHit && _gameActive);

        //  Activates the buttons for the player until the game ends
        if (_gameActive)
        {
            foreach (var btn in BotGrid.Children.OfType<Button>())
            {
                btn.InputTransparent = false;
            }
        }
    }

    private async void WinCheck(Person attacker, Person target)
    {
        Person? winner;
        if (target.Ships.All(s => s.Sunken))
        {
            attacker.HasWon = true;
            winner = attacker;
        }
        else
        {
            winner = null;
        }
        
        if (winner != null)
        {
            await Task.Delay(2000);
            _gameActive = false;
            attacker.HasAHit = false;
        }
    }

    private void BuildBotGrid()
    {
        for (int i = 0; i < 10; i++)
        {
            BotGrid.RowDefinitions.Add(new RowDefinition{ Height = GridLength.Star});    //  Star = Equal parts
            BotGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width = GridLength.Star});
        }
        for (int col = 0; col < 10; col++)
        {
            for (int row = 0; row < 10; row++)
            {
                var cellButton = new Button
                {
                    BackgroundColor = Color.FromArgb("#0A192F"),
                    BorderColor = Color.FromArgb("#1E3A5F"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    Padding = 0,
                    Margin = 1,

                    // Coordinates on the button  ||  (char)('A' + r) converts the r index to letter by summing it up with A's index
                    BindingContext = _bot.Board.GetCoordinate(col + 1, (char)('A' + row))
                };
                
                //  Unified event handler for all cells
                cellButton.Clicked += OnCellClicked;

                //  Places the cellbtn into the visual row and column
                Grid.SetRow(cellButton, row);
                Grid.SetColumn(cellButton, col);

                BotGrid.Children.Add(cellButton);
            }
        }
    }

    private void ShipHit(Coordinate coord, Person person)
    {
        Grid grid = person == _bot ? BotGrid : PlayerGrid; 
        var shipThatIsHit = person.Ships.First(s => s.Sunken && s.Location.Any(x => x == coord));
        foreach (var c in shipThatIsHit.Location)
        {
            Button btn = grid.Children.OfType<Button>().First(b => b.BindingContext == c);
            SunkenShipFeedback(btn);
        }
    }

    private void SunkenShipFeedback(Button btn)
    {
        btn.BorderColor = Colors.Cyan;
        btn.TextColor = Colors.Cyan;
    }
}