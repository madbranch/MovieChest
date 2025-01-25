using System.ComponentModel;

namespace MovieChest.ComponentModel;

public static class NotifyPropertyChangedExtensions
{
    public static PropertySubscriber<T> When<T>(this T subject)
        where T : INotifyPropertyChanged
        => new PropertySubscriber<T>(subject);
}