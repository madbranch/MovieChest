using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Collections.Generic;

namespace MovieChest;

public partial class EditMovieDialog : Window
{
    public EditMovieDialog()
        => InitializeComponent();

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
        => Close(false);

    private void OkButton_Click(object? sender, RoutedEventArgs e)
        => Close(true);

    private async void BrowseButton_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not EditMovieViewModel viewModel)
        {
            return;
        }
        FilePickerOpenOptions options = new()
        {
            Title = "Select Movie File",
            AllowMultiple = false,
        };
        IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(options);
        if (files.Count <= 0)
        {
            return;
        }
        viewModel.Path = files[0].TryGetLocalPath();
    }
}