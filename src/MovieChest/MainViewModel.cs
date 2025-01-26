using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MovieChest.ComponentModel;
using System.Collections.ObjectModel;

namespace MovieChest;

public partial class MovieItem : ObservableObject
{
    [ObservableProperty]
    private string title = "";

    [ObservableProperty]
    private string description = "";
}

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        Movies.Add(new MovieItem { Title = "Kung Pow", Description = "Best movie ever." });
        Movies.Add(new MovieItem { Title = "Up", Description = "Best animation movie ever." });
    }

    public ObservableCollection<MovieItem> Movies { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteSelectedMovieCommand))]
    private MovieItem? selectedMovie;

    [RelayCommand(CanExecute = nameof(CanDeleteSelectedMovie))]
    private void DeleteSelectedMovie()
    {
        if (SelectedMovie is not MovieItem selectedMovie)
        {
            return;
        }
        Movies.Remove(selectedMovie);
        SelectedMovie = null;
    }

    private bool CanDeleteSelectedMovie()
        => SelectedMovie is not null;
}