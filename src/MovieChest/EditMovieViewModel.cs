using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace MovieChest;

public partial class EditMovieViewModel : ObservableValidator
{
    private readonly IDriveInfoProvider driveInfoProvider;

    public EditMovieViewModel(IDriveInfoProvider driveInfoProvider)
    {
        this.driveInfoProvider = driveInfoProvider;
    }

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
    private Uri? path;

    partial void OnPathChanged(Uri? value)
        => VolumeLabel = value switch
        {
            null => "",
            Uri path => GetVolumeLabel(path.AbsolutePath),
        };

    private string GetVolumeLabel(string absolutePath)
    {
        IOrderedEnumerable<DriveInfo> drives = driveInfoProvider.GetDrives().OrderByDescending(x => x.RootDirectory.FullName.Length);
        foreach (DriveInfo drive in drives)
        {
            if (absolutePath.StartsWith(drive.RootDirectory.FullName))
            {
                return drive.VolumeLabel;
            }
        }
        return "";
    }

    [ObservableProperty]
    private string volumeLabel = "";

}