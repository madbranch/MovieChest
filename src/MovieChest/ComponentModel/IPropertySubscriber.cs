using System;
using System.ComponentModel;

namespace MovieChest.ComponentModel;

public interface IPropertySubscriber<T>
    where T : INotifyPropertyChanged
{
    IPropertySubscriber<T> Any(string[] properties, Action doThis);
    IDisposable Subscribe();
}