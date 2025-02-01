using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MovieChest.ComponentModel;

public class ViewModelBase : ObservableValidator, ICompositeDisposable
{
    private readonly CompositeDisposable compositeDisposable = new();

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            compositeDisposable.Dispose();
        }
    }

    ~ViewModelBase()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    void ICompositeDisposable.Add(IDisposable disposable)
        => compositeDisposable.Add(disposable);

    void ICompositeDisposable.Remove(IDisposable disposable)
        => compositeDisposable.Remove(disposable);
}