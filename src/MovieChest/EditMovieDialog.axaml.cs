using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MovieChest;

public partial class EditMovieDialog : Window
{
    public EditMovieDialog()
    {
        InitializeComponent();
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }

    private void OkButton_Click(object? sender, RoutedEventArgs e)
    {
        Close(true);
    }
}