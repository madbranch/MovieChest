using MovieChest.ComponentModel;
using System;
using System.Threading;

namespace MovieChest;

public sealed class AvaloniaDispatcher(Avalonia.Threading.Dispatcher dispatcher) : IDispatcher
{
    public bool IsSynchronized => dispatcher.CheckAccess();

    public void BeginInvoke(Action action)
        => dispatcher.Post(action);

    public void BeginInvoke(SendOrPostCallback callback, object? arg)
        => dispatcher.Post(callback, arg);

    public void Invoke(Action action)
        => dispatcher.Invoke(action);

    public void Invoke(SendOrPostCallback callback, object? arg)
        => dispatcher.Invoke(() => callback(arg));

    public T Invoke<T>(Func<T> function) => dispatcher.Invoke(function);
}
