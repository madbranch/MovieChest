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
}