using MovieChest.ComponentModel;
using System;

namespace MovieChest;

public sealed class AvaloniaDispatcher : IDispatcher
{
    private readonly Avalonia.Threading.Dispatcher dispatcher = Avalonia.Threading.Dispatcher.UIThread;

    public bool IsSynchronized => dispatcher.CheckAccess();

    public void BeginInvoke(Delegate method, params object[] args) => throw new NotImplementedException();
    public void Invoke(Action action) => dispatcher.Invoke(action);
    public object Invoke(Delegate method, params object[] args) => throw new NotImplementedException();
    public T Invoke<T>(Func<T> function) => dispatcher.Invoke(function);
}
