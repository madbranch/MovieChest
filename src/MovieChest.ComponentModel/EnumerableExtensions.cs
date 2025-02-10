using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MovieChest.ComponentModel;

public static class EnumerableExtensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
        => new ObservableCollection<T>(items);
}