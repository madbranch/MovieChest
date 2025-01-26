using System;

namespace MovieChest.ComponentModel;

public interface ICompositeDisposable : IDisposable
{
    void Add(IDisposable disposable);
    void Remove(IDisposable disposable);
}