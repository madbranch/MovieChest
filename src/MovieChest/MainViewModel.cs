using CommunityToolkit.Mvvm.ComponentModel;
using MovieChest.ComponentModel;

namespace MovieChest;

public partial class MainViewModel : ViewModelBase
{
    private readonly IMovieCollectionSerializer _movieCollectionSerializer;

    public MainViewModel(IMovieCollectionSerializer movieCollectinSerializer)
    {
        _movieCollectionSerializer = movieCollectinSerializer;
    }
}

public partial class AdamViewModel : ViewModelBase
{
    public AdamViewModel()
    {
        this.When()
            .Any( [nameof(PropertyA), nameof(PropertyB)], UpdatePropertyC )
            .Subscribe();
    }

    [ObservableProperty]
    private string propertyA = "";

    [ObservableProperty]
    private string propertyB = "";

    [ObservableProperty]
    private string propertyC = "";

    private void UpdatePropertyC()
    {
        PropertyC = PropertyA + PropertyB;
    }
}
