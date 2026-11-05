using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Health.Connect.DataTypes.Units;

namespace Battleship.Maui;

public partial class DeployPage : ContentPage
{
    private Player _player = new();
    private List<Coordinate> _currentShipLocation = new();
    private Ship _currentShip;

    public DeployPage()
    {
        InitializeComponent();
        UpdateInventoryUI();
        BuildBoardGrid();
    }
    
    private void UpdateInventoryUI()
    {
        ShipInventory.Children.Clear();
        ShipInventory.RowDefinitions.Clear();

        Ship[] allShips =
        {
            new Carrier(_player, new List<Coordinate>()),
            new Battleship(_player, new List<Coordinate>()),
            new Destroyer(_player, new List<Coordinate>()),
            new Submarine(_player, new List<Coordinate>()),
            new PatrolBoat(_player, new List<Coordinate>())
        };

        var remainingShips = allShips.Where(s => !_player._ships.Any(ps => ps.Name == s.Name)).ToArray();

        foreach (var unused in remainingShips)
        {
            ShipInventory.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
        }

        for (int row = 0; row < remainingShips.Length; row++)
        {
            var ship = remainingShips[row];

            var shipButton = new Button
            {
                Text = ship.Name,
                BackgroundColor = Color.FromArgb("#020B14"),
                BorderColor = Color.FromArgb("#1E3A5F"),
                BorderWidth = 1,
                FontSize = 10,
                Padding = 2,
                Margin = 2,
                BindingContext = ship
            };

            shipButton.Clicked += OnShipClicked;
            
            Grid.SetRow(shipButton, row);
            Grid.SetColumn(shipButton, 0);
            
            ShipInventory.Children.Add(shipButton);
        }
    }

    private void BuildBoardGrid()
    {
        //  Add 10 rows and 10 columns to the DeployGrid
        for (int i = 0; i < 10; i++)
        {
            DeployGrid.RowDefinitions.Add(new RowDefinition{ Height = GridLength.Star});
            DeployGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width = GridLength.Star});
        }
        
        //  Populate all those 10x10 cells created
        for (int r = 0; r < 10; r++)
        {
            for (int c = 0; c < 10; c++)
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
                    BindingContext = _player.Board.GetCoordinate(c + 1, (char)('A' + r))
                };
                
                //  Unified event handler for all cells
                cellButton.Clicked += OnCellClicked;

                //  Places the cellbtn into the visual row and column
                Grid.SetRow(cellButton, r);
                Grid.SetColumn(cellButton, c);

                DeployGrid.Children.Add(cellButton);
            }
        }
    }

    private void OnShipClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext != null)
        {
            dynamic chosenShip = btn.BindingContext;
            SetCurrentShip(chosenShip);
        }
    }

    private void SetCurrentShip(Ship ship)
    {
        _currentShip = ship;
    }
        
    private async void OnCellClicked(object? sender, EventArgs e)
    {
        //  Null protection in case coordinates on the button are not set
        if (sender is Button btn && btn.BindingContext != null)
        {
            dynamic c = btn.BindingContext;

            if (await RuleCheck(c))
            {
                _currentShipLocation.Add(c);

                if (_currentShipLocation.Count == GetLenght())
                {
                    foreach (var coord in _currentShipLocation)
                    {
                        coord.ChangeStatus("Ship");
                    }

                    _currentShip.Location = _currentShipLocation;
                    _player.AddShip(_currentShip);
                    _currentShip = null;
                    _currentShipLocation.Clear();
                    
                    UpdateInventoryUI();
                }
                
                //  Feedback
                btn.Text = "S";
                btn.TextColor = Colors.White;
                btn.FontAttributes = FontAttributes.Bold;
                btn.BorderColor = Colors.Cyan;
            }
        }
    }

    private async Task<bool> RuleCheck(Coordinate c) 
    {
        int length = GetLenght();

        //  If no ship is chosen
        if (length == 0)
        {
            return false;
        }
        
        if (c.ToText() != "|_S|")
        {
            if (_currentShipLocation.Count == 0)
            {
                return true;
            }
            // else
            //{
                if (Coordinate.CIsNextToAnotherC(c, _currentShipLocation.Last()))
                {
                    bool isHorizontal = c.YValue == _currentShipLocation.First().YValue;

                    string direction = c.YValue > _currentShipLocation.First().YValue ? "Downwards" : "Upwards";

                    var pool = _player.Board.CoordinateLayout.Where(x => x.XValue == c.XValue && x.YValue <= c.YValue && x.ToText() == "|__|");
                    
                    if (direction == "Downwards")
                    {
                        pool = _player.Board.CoordinateLayout.Where(x => x.XValue == c.XValue && x.YValue >= c.YValue && x.ToText() == "|__|");
                    }
                    
                    if (isHorizontal)
                    {
                        direction = c.XValue > _currentShipLocation.First().XValue ? "To right" : "To left";
                        
                        pool = _player.Board.CoordinateLayout.Where(x => x.YValue == c.YValue && x.XValue <= c.XValue && x.ToText() == "|__|");
                        
                        if (direction == "To right")
                        {
                            pool = _player.Board.CoordinateLayout.Where(x => x.YValue == c.YValue && x.XValue >= c.XValue && x.ToText() == "|__|");
                        }
                    }
                    
                    if (pool.Count() >= length - _currentShipLocation.Count)
                    {
                        if (_currentShipLocation.Count == 1)
                        {
                            return true;
                        }
                        //else
                        //{
                        if (_currentShipLocation[0].XValue == c.XValue || _currentShipLocation[0].YValue == c.YValue)
                        {
                            return true;
                        }
                        //else
                        //{
                        await DisplayAlert("Error",
                            "The chosen coordinate has to line up straight with the previous chosen coordinates, please try again.",
                            "Try again");
                        //}
                    }
                    else
                    {
                        await DisplayAlert("Error",
                            "The ship must make a straight line and there has to be place left for the rest of the ship after the chosen coordinate, please try again.",
                            "Try again");
                    }
                }
                else
                {
                await DisplayAlert("Error", "The chosen coordinate has to be next to the last one, please try again.",
                    "Try again");
                }
        }
        else
        {
            await DisplayAlert("Error", "The chosen coordinate is already assigned to another ship, please try again.", "Try again");
        }

        return false;
    }

    private int GetLenght()
    {
        if (_currentShip == null) return 0;
        
        switch (_currentShip.Name)
        {
            case "Carrier":
                return 5;
            case "Battleship":
                return 4;
            case "Destroyer":
                return 3;
            case "Submarine":
                return 3;
            case "Patrol Boat":
                return 2;
            default:
                return 0;
        }
    }
}