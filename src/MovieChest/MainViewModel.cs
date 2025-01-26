using MovieChest.ComponentModel;

namespace MovieChest;

public partial class MainViewModel : ViewModelBase
{
    private readonly IMovieCollectionSerializer movieCollectionSerializer;

    public MainViewModel(IMovieCollectionSerializer movieCollectinSerializer)
    {
        this.movieCollectionSerializer = movieCollectinSerializer;
    }
}
