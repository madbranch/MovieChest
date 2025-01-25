using System;

namespace MovieChest.ComponentModel;

public interface IDispatcher
{
    bool IsSynchronized { get; }
    void BeginInvoke(Delegate method, params object[] args);
    void Invoke(Action action);
    object Invoke(Delegate method, params object[] args);
    T Invoke<T>(Func<T> function);
}