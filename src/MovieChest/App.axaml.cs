using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using System;

namespace MovieChest;

public partial class App : Application
{
    private MainViewModel? _viewModel;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        RemoveDataAnnotationsValidator();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            if (_viewModel is not null)
            {
                throw new InvalidOperationException();
            }

            _viewModel = CreateViewModel();
            desktop.Exit += Desktop_Exit;
            desktop.MainWindow = new MainWindow
            {
                DataContext = _viewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void RemoveDataAnnotationsValidator()
    {
        int i = 0;
        while (i < BindingPlugins.DataValidators.Count)
        {
            if (BindingPlugins.DataValidators[i] is DataAnnotationsValidationPlugin)
                BindingPlugins.DataValidators.RemoveAt(i);
            else
                ++i;
        }
    }

    private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        if (sender is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Exit -= Desktop_Exit;
        }
    }

    private MainViewModel CreateViewModel()
    {
        SystemDriveInfoProvider driveInfoProvider = new();
        MovieSerializer movieSerializer = new();
        MainViewModel viewModel = new(() => new EditMovieViewModel(driveInfoProvider), movieSerializer);
        return viewModel;
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}