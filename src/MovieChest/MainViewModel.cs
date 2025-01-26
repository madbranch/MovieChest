using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MovieChest.ComponentModel;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

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

        this.When()
            .Any([nameof(MovieFilter)], UpdateFilteredMovies)
            .Subscribe()
            .DisposeWith(this);

        filteredMovies = GetFilteredMovies();
    }

    public ObservableCollection<MovieItem> Movies { get; } = [];

    [ObservableProperty]
    private string? movieFilter;

    [ObservableProperty]
    private ImmutableArray<MovieItem> filteredMovies = [];

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
        UpdateFilteredMovies();
    }

    private bool CanDeleteSelectedMovie()
        => SelectedMovie is not null;

    private ImmutableArray<MovieItem> GetFilteredMovies()
        => string.IsNullOrWhiteSpace(MovieFilter)
        ? Movies.ToImmutableArray()
        : Movies.Where(x => x.Title.Contains(MovieFilter, StringComparison.CurrentCultureIgnoreCase)).ToImmutableArray();

    private void UpdateFilteredMovies()
        => FilteredMovies = GetFilteredMovies();
}