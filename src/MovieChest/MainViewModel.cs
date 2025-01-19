using ReactiveUI;

namespace MovieChest;

public partial class MainViewModel : ReactiveObject
{
    public string Greeting { get; } = "Welcome to Avalonia!";
}