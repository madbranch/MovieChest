using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MovieChest.ComponentModel;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MovieChest;

public partial class MovieItem : ObservableObject
{
    [ObservableProperty]
    private string title = "";

    [ObservableProperty]
    private string description = "";

    [ObservableProperty]
    private string tags = "";
}

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Movies.Add(new MovieItem { Title = "Kung Pow", Description = "Best movie ever." });
        Movies.Add(new MovieItem { Title = "Up", Description = "Best animation movie ever." });

        filteredMovies = GetFilteredMovies();
    }

    public ObservableCollection<MovieItem> Movies { get; } = [];

    [ObservableProperty]
    private string movieFilter = "";

    partial void OnMovieFilterChanged(string value)
        => UpdateFilteredMovies();

    [ObservableProperty]
    private ImmutableArray<MovieItem> filteredMovies = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteSelectedMovieCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditSelectedMovieCommand))]
    private MovieItem? selectedMovie;

    [RelayCommand(CanExecute = nameof(CanDeleteSelectedMovie))]
    private async Task DeleteSelectedMovieAsync()
    {
        if (SelectedMovie is not MovieItem selectedMovie)
        {
            return;
        }

        if (await confirmMovieDeletion.HandleAsync(selectedMovie) == MovieDeletionConfirmation.DontDeleteMovie)
        {
            return;
        }

        Movies.Remove(selectedMovie);
        SelectedMovie = null;
        UpdateFilteredMovies();
    }

    private bool CanDeleteSelectedMovie()
        => SelectedMovie is not null;

    public IInteraction<MovieItem, MovieDeletionConfirmation> ConfirmMovieDeletion => confirmMovieDeletion;
    private readonly Interaction<MovieItem, MovieDeletionConfirmation> confirmMovieDeletion = new();

    [RelayCommand(CanExecute = nameof(CanEditSelectedMovie))]
    private async Task EditSelectedMovieAsync()
    {
        if (SelectedMovie is not MovieItem selectedMovie)
        {
            return;
        }

        if (await editMovie.HandleAsync(selectedMovie) is not MovieItem editedMovie)
        {
            return;
        }

        int selectedMovieIndex = Movies.IndexOf(selectedMovie);
        
        if (selectedMovieIndex == -1)
        {
            return;
        }

        SelectedMovie = null;
        Movies[selectedMovieIndex] = editedMovie;
        UpdateFilteredMovies();
        SelectedMovie = editedMovie;
    }

    private bool CanEditSelectedMovie()
        => SelectedMovie is not null;

    [RelayCommand]
    private async Task AddMovieAsync()
    {
        MovieItem newMovie = new()
        {
            Title = "Movie Title",
            Description = "Movie Description",
        };
        if (await editMovie.HandleAsync(newMovie) is not MovieItem editedMovie)
        {
            return;
        }

        SelectedMovie = null;
        Movies.Add(newMovie);
        SelectedMovie = newMovie;
        UpdateFilteredMovies();
    }

    public IInteraction<MovieItem, MovieItem?> EditMovie => editMovie;
    private readonly Interaction<MovieItem, MovieItem?> editMovie = new();

    public IInteraction<MovieItem, MovieItem?> AddMovie => addMovie;
    private readonly Interaction<MovieItem, MovieItem?> addMovie = new();

    private ImmutableArray<MovieItem> GetFilteredMovies()
        => string.IsNullOrWhiteSpace(MovieFilter)
        ? Movies.ToImmutableArray()
        : Movies.Where(IsMovieVisible).ToImmutableArray();

    private bool IsMovieVisible(MovieItem movie)
        => movie.Title.Contains(MovieFilter, StringComparison.CurrentCultureIgnoreCase)
        || movie.Description.Contains(MovieFilter, StringComparison.CurrentCultureIgnoreCase)
        || movie.Tags.Contains(MovieFilter, StringComparison.CurrentCultureIgnoreCase);

    private void UpdateFilteredMovies()
        => FilteredMovies = GetFilteredMovies();
}