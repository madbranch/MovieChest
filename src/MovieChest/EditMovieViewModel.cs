using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieChest;

public partial class EditMovieViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    private string title = "";

    [ObservableProperty]
    [Required]
    [MaxLength(2000)]
    private string description = "";

    [ObservableProperty]
    [MaxLength(2000)]
    private string tags = "";

    [ObservableProperty]
    [Required]
    private string path = "";

    [ObservableProperty]
    [Required]
    private string volumeLabel = "";
}