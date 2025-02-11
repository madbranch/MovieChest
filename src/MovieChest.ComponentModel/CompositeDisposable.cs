using System;

namespace MovieChest.ComponentModel;

public sealed class CompositeDisposable : ICompositeDisposable
{
    private Action? disposables;

    public void Add(IDisposable disposable)
        => disposables = disposable.Dispose + disposables;

    public void Dispose()
    {
        disposables?.Invoke();
        disposables = null;
    }

    public bool Remove(IDisposable disposable)
    {
        if ((disposables -= disposable.Dispose) == disposables)
        {
            return false;
        }

        disposable.Dispose();
        return true;
    }
}