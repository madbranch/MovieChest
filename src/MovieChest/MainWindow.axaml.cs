using Avalonia.Controls;

namespace MovieChest;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated<MainViewModel>((vm, d) =>
        {
        });
    }

    private string? SelectDatabase(string initialDirectory)
    {
        return null;
    }
}