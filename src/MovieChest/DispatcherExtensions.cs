using MovieChest.ComponentModel;

namespace MovieChest;

public static class DispatcherExtensions
{
    public static IDispatcher Wrap(this Avalonia.Threading.Dispatcher dispatcher)
        => new AvaloniaDispatcher(dispatcher);
}