using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Maui;

public partial class DeployPage : ContentPage
{
    private Player _player = new();
    public DeployPage()
    {
        InitializeComponent();
        BuildBoardGrip();
    }

    private void BuildBoardGrip()
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

    private void OnCellClicked(object? sender, EventArgs e)
    {
        //  Null protection in case coordinates on the button are not set
        if (sender is Button btn && btn.BindingContext != null)
        {
            dynamic c = btn.BindingContext;
            int x = c.X;
            int y = c.Y;
            
            //  Feedback
            btn.BorderColor = Colors.Cyan;
        }
    }
    
    
}