using System;

namespace MovieChest.ComponentModel;

public interface ICompositeDisposable : IDisposable
{
    void Add(IDisposable disposable);
    bool Remove(IDisposable disposable);
}