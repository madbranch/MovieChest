using Avalonia.Controls;
using MovieChest.ComponentModel;
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
        });
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

    private async Task<MovieItem?> EditMovieAsync(MovieItem movie)
    {
        EditMovieViewModel viewModel = new()
        {
            Title = movie.Title,
            Description = movie.Description,
        };
        EditMovieDialog view = new() { DataContext = viewModel };
        if (await view.ShowDialog<bool?>(this) != true)
        {
            return null;
        }
        return new MovieItem
        {
            Title = viewModel.Title,
            Description = viewModel.Description,
        };
    }

    private async Task<MovieItem?> AddMovieAsync(MovieItem movie)
    {
        EditMovieViewModel viewModel = new()
        {
            Title = movie.Title,
            Description = movie.Description,
        };
        EditMovieDialog view = new() { DataContext = viewModel };
        view.Title = "New Movie";
        if (await view.ShowDialog<bool?>(this) != true)
        {
            return null;
        }
        return new MovieItem
        {
            Title = viewModel.Title,
            Description = viewModel.Description,
        };
    }
}