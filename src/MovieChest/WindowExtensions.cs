using Avalonia.Controls;
using Avalonia.Interactivity;
using MovieChest.ComponentModel;
using System;

namespace MovieChest;

public static class WindowExtensions
{
    public static void WhenActivated<TViewModel>(this Window window, Action<TViewModel, ICompositeDisposable> doThis )
        where TViewModel : class
    {
        CompositeDisposable? compositeDisposable = null;
        TViewModel? viewModel = null;

        if (window.IsLoaded)
        {
            SetViewModel(window.DataContext as TViewModel);
            window.Unloaded += Window_Unloaded;
            window.DataContextChanged += Window_DataContextChanged;
        }
        else
        {
            window.Loaded += Window_Loaded;
        }

        void Window_Loaded(object? sender, RoutedEventArgs e)
        {
            Window window = (Window)sender!;
            window.Loaded -= Window_Loaded;
            window.Unloaded += Window_Unloaded;
            window.DataContextChanged += Window_DataContextChanged;
            SetViewModel(window.DataContext as TViewModel);
        }

        void Window_Unloaded(object? sender, RoutedEventArgs e)
        {
            Window window = (Window)sender!;
            window.Unloaded -= Window_Unloaded;
            window.DataContextChanged -= Window_DataContextChanged;
            SetViewModel(null);
        }

        void Window_DataContextChanged(object? sender, EventArgs e)
        {
            Window window = (Window)sender!;
            SetViewModel(window.DataContext as TViewModel);
        }

        void SetViewModel(TViewModel? newValue)
        {
            if (newValue == viewModel)
            {
                return;
            }
            compositeDisposable?.Dispose();
            viewModel = newValue;
            if (viewModel is null)
            {
                return;
            }
            doThis(viewModel, compositeDisposable = new CompositeDisposable());
        }
    }
}