using System;
using System.Collections.Generic;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Project.ViewModels;

namespace Project.Views.Popups;

public partial class AddEditCategoryPopup : Popup
{
    private readonly AddEditCategoryViewModel _vm;
    private readonly Dictionary<string, Border> _swatches = [];

    public AddEditCategoryPopup(AddEditCategoryViewModel vm)
    {
        _vm = vm;
        BindingContext = vm;
        InitializeComponent();
        BuildColorGrid();

        vm.PropertyChanged += async (_, e) =>
        {
            if (e.PropertyName == nameof(AddEditCategoryViewModel.SaveSucceeded) && vm.SaveSucceeded)
                await CloseAsync();
        };
    }

    private void BuildColorGrid()
    {
        ColorGrid.Children.Clear();
        _swatches.Clear();

        foreach (var hex in _vm.PresetColors)
        {
            // Outer border = selection ring
            var ring = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = 16 },
                StrokeThickness = 0,
                BackgroundColor = Colors.Transparent,
                Padding = new Thickness(2),
                Margin = new Thickness(3),
                WidthRequest = 34,
                HeightRequest = 34
            };

            // Inner circle = the color
            var circle = new Button
            {
                BackgroundColor = Color.FromArgb(hex),
                CornerRadius = 13,
                WidthRequest = 26,
                HeightRequest = 26,
                MinimumWidthRequest = 0,
                MinimumHeightRequest = 0,
                Padding = new Thickness(0),
                BorderWidth = 0
            };
            circle.Clicked += (_, _) => SelectColor(hex);

            ring.Content = circle;
            _swatches[hex] = ring;
            ColorGrid.Add(ring);
        }

        UpdateSelectionRing();
    }

    private void SelectColor(string hex)
    {
        _vm.SelectedColor = hex;
        UpdateSelectionRing();
    }

    private void UpdateSelectionRing()
    {
        foreach (var (hex, ring) in _swatches)
        {
            if (hex == _vm.SelectedColor)
            {
                ring.Stroke = new SolidColorBrush(Color.FromArgb(_vm.SelectedColor));
                ring.StrokeThickness = 3;
                ring.BackgroundColor = Color.FromArgb(hex).WithAlpha(0.20f);
            }
            else
            {
                ring.StrokeThickness = 0;
                ring.BackgroundColor = Colors.Transparent;
            }
        }
    }

    private async void OnCancelClicked(object? sender, EventArgs e) => await CloseAsync();
}
