using System;

namespace MovieChest.ComponentModel;

public static class DisposableExtensions
{
    public static T DisposeWith<T>(this T disposable, ICompositeDisposable compositeDisposable)
        where T : IDisposable
    {
        compositeDisposable.Add(disposable);
        return disposable;
    }

    public static void WhenDisposed(this ICompositeDisposable compositeDisposable, Action doThis)
        => compositeDisposable.Add(new ActionDisposable(doThis));
}