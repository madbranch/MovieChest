using CommunityToolkit.Mvvm.ComponentModel;
using MovieChest.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieChest;

public partial class EditMovieViewModel : ViewModelBase
{
    [ObservableProperty]
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    private string title = "";

    [ObservableProperty]
    [Required]
    [MaxLength(2000)]
    private string description = "";
}