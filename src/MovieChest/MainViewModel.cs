using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MovieChest.ComponentModel;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MovieChest;

public record MovieItem(string Title, string Description, string Tags, string? Path, string VolumeLabel);

public partial class MainViewModel : ObservableObject
{
    private readonly Func<EditMovieViewModel> editMovieViewModelFactory;
    private readonly IMovieSerializer movieSerializer;

    public MainViewModel(Func<EditMovieViewModel> editMovieViewModelFactory, IMovieSerializer movieSerializer)
    {
        Movies.Add(new MovieItem("Kung Pow", "Best movie ever.", "", null, ""));
        Movies.Add(new MovieItem("Up", "Best animation movie ever.", "", null, ""));

        filteredMovies = GetFilteredMovies();
        this.editMovieViewModelFactory = editMovieViewModelFactory;
        this.movieSerializer = movieSerializer;
    }

    [ObservableProperty]
    private ObservableCollection<MovieItem> movies = [];

    partial void OnMoviesChanged(ObservableCollection<MovieItem> value)
        => UpdateFilteredMovies();
    
    [ObservableProperty]
    private string movieFilter = "";

    partial void OnMovieFilterChanged(string value)
        => UpdateFilteredMovies();

    [ObservableProperty]
    private string? movieChestFile;

    [ObservableProperty]
    private ImmutableArray<MovieItem> filteredMovies = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteSelectedMovieCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditSelectedMovieCommand))]
    private MovieItem? selectedMovie;

    [RelayCommand]
    private async Task OpenMovieChestFile()
    {
        if (await selectMovieChestFile.HandleAsync(null) is not string selectedMovieChestFile)
        {
            return;
        }
        SelectedMovie = null;
        MovieChestFile = selectedMovieChestFile;
        Movies = movieSerializer.GetMovies(selectedMovieChestFile).ToObservableCollection();
    }

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
        SerializeMovies();
    }

    private bool CanDeleteSelectedMovie()
        => SelectedMovie is not null;

    public IInteraction<string?, string?> SelectMovieChestFile => selectMovieChestFile;
    private readonly Interaction<string?, string?> selectMovieChestFile = new();

    public IInteraction<MovieItem, MovieDeletionConfirmation> ConfirmMovieDeletion => confirmMovieDeletion;
    private readonly Interaction<MovieItem, MovieDeletionConfirmation> confirmMovieDeletion = new();

    [RelayCommand(CanExecute = nameof(CanEditSelectedMovie))]
    private async Task EditSelectedMovieAsync()
    {
        if (SelectedMovie is not MovieItem selectedMovie)
        {
            return;
        }
        EditMovieViewModel viewModel = editMovieViewModelFactory();
        viewModel.Title = selectedMovie.Title;
        viewModel.Description = selectedMovie.Description;
        viewModel.Tags = selectedMovie.Tags;
        viewModel.Path = selectedMovie.Path;
        viewModel.VolumeLabel = selectedMovie.VolumeLabel;
        if (await editMovie.HandleAsync(viewModel) is not EditMovieViewModel editedViewModel)
        {
            return;
        }
        int selectedMovieIndex = Movies.IndexOf(selectedMovie);
        if (selectedMovieIndex == -1)
        {
            return;
        }
        SelectedMovie = null;
        MovieItem editedMovie = new MovieItem(editedViewModel.Title, editedViewModel.Description, editedViewModel.Tags, editedViewModel.Path, editedViewModel.VolumeLabel);
        Movies[selectedMovieIndex] = editedMovie;
        UpdateFilteredMovies();
        SelectedMovie = editedMovie;
        SerializeMovies();
    }

    private bool CanEditSelectedMovie()
        => SelectedMovie is not null;

    [RelayCommand]
    private async Task AddMovieAsync()
    {
        EditMovieViewModel viewModel = editMovieViewModelFactory();
        viewModel.Title = "Movie Title";
        viewModel.Description = "Movie Description";
        if (await addMovie.HandleAsync(viewModel) is not EditMovieViewModel editedViewModel)
        {
            return;
        }
        MovieItem newMovie = new(editedViewModel.Title, editedViewModel.Description, editedViewModel.Tags, editedViewModel.Path, editedViewModel.VolumeLabel);
        SelectedMovie = null;
        Movies.Add(newMovie);
        UpdateFilteredMovies();
        SelectedMovie = newMovie;
        SerializeMovies();
    }

    public IInteraction<EditMovieViewModel, EditMovieViewModel?> EditMovie => editMovie;
    private readonly Interaction<EditMovieViewModel, EditMovieViewModel?> editMovie = new();

    public IInteraction<EditMovieViewModel, EditMovieViewModel?> AddMovie => addMovie;
    private readonly Interaction<EditMovieViewModel, EditMovieViewModel?> addMovie = new();

    private ImmutableArray<MovieItem> GetFilteredMovies()
        => string.IsNullOrWhiteSpace(MovieFilter)
        ? Movies.ToImmutableArray()
        : Movies.Where(x => x.Title.Contains(MovieFilter, StringComparison.CurrentCultureIgnoreCase)).ToImmutableArray();

    private bool IsMovieVisible(MovieItem movie)
        => movie.Title.Contains(MovieFilter, StringComparison.CurrentCultureIgnoreCase)
        || movie.Description.Contains(MovieFilter, StringComparison.CurrentCultureIgnoreCase)
        || movie.Tags.Contains(MovieFilter, StringComparison.CurrentCultureIgnoreCase);

    private void UpdateFilteredMovies()
        => FilteredMovies = GetFilteredMovies();

    private void SerializeMovies()
    {
        if (MovieChestFile is not string movieChestFile)
        {
            return;
        }
        movieSerializer.SetMovies(movieChestFile, Movies);
    }
}