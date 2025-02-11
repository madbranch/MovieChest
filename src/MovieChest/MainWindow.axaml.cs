using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MovieChest.ComponentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieChest;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated<MainViewModel>((vm, d) =>
        {
            vm.ConfirmMovieDeletion.Register(ConfirmMovieDeletionAsync)
                .DisposeWith(d);
            vm.EditMovie.Register(EditMovieAsync)
                .DisposeWith(d);
            vm.AddMovie.Register(AddMovieAsync)
                .DisposeWith(d);
            vm.SelectMovieChestFile.Register(SelectMovieChestFileAsync)
                .DisposeWith(d);
        });
    }

    private async Task<string?> SelectMovieChestFileAsync(string? pth)
    {
        FilePickerOpenOptions options = new()
        {
            Title = "Select Movie File",
            AllowMultiple = false,
        };
        IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(options);
        if (files.Count <= 0)
        {
            return null;
        }
        return files[0].TryGetLocalPath();
    }

    private string? SelectDatabase(string initialDirectory)
    {
        return null;
    }

    private async Task<MovieDeletionConfirmation> ConfirmMovieDeletionAsync(MovieItem movie)
    {
        MessageBoxViewModel viewModel = new([new MessageBoxChoice("DontDeleteMovie", "Cancel"), new MessageBoxChoice("DeleteMovie", "Delete")], $"Do you want to delete the movie {movie.Title}?");
        MessageBox view = new() { DataContext = viewModel };
        await view.ShowDialog(this);
        return viewModel.SelectedChoice?.Id switch
        {
            "DeleteMovie" => MovieDeletionConfirmation.DeleteMovie,
            _ => MovieDeletionConfirmation.DontDeleteMovie,
        };
    }

    private async Task<EditMovieViewModel?> EditMovieAsync(EditMovieViewModel viewModel)
    {
        EditMovieDialog view = new() { DataContext = viewModel };
        return await view.ShowDialog<bool?>(this) == true
            ? viewModel
            : null;
    }

    private async Task<EditMovieViewModel?> AddMovieAsync(EditMovieViewModel viewModel)
    {
        EditMovieDialog view = new() { DataContext = viewModel };
        view.Title = "New Movie";
        return await view.ShowDialog<bool?>(this) == true
            ? viewModel
            : null;
    }
}