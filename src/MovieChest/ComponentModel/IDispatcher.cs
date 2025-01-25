using System;
using System.Threading;

namespace MovieChest.ComponentModel;

public interface IDispatcher
{
    bool IsSynchronized { get; }
    void BeginInvoke(Action action);
    void BeginInvoke(SendOrPostCallback callback, object? arg);
    void Invoke(Action action);
    void Invoke(SendOrPostCallback callback, object? arg);
    T Invoke<T>(Func<T> function);

    void SafeBeginInvoke(Action action)
    {
        if (IsSynchronized)
            action();
        else
            BeginInvoke(action);
    }

    void SafeBeginInvoke(SendOrPostCallback callback, object? arg)
    {
        if (IsSynchronized)
            callback(arg);
        else
            BeginInvoke(callback, arg);
    }

    void SafeInvoke(Action action)
    {
        if (IsSynchronized)
            action();
        else
            Invoke(action);
    }

    void SafeInvoke(SendOrPostCallback callback, object? arg)
    {
        if (IsSynchronized)
            callback(arg);
        else
            Invoke(callback, arg);
    }

    T SafeInvoke<T>(Func<T> function)
        => IsSynchronized ? function() : SafeInvoke(function);
}