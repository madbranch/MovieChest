using System;

namespace MovieChest.ComponentModel;

public sealed class CompositeDisposable : ICompositeDisposable
{
    private Action? disposables;
    public void Add(IDisposable disposable)
        => disposables = disposable.Dispose + disposables;
    public void Dispose()
        => disposables?.Invoke();
    public void Remove(IDisposable disposable)
        => disposables -= disposable.Dispose;
}