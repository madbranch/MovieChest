using System.IO;

namespace MovieChest;

public class SystemDriveInfoProvider : IDriveInfoProvider
{
    public DriveInfo[] GetDrives()
        => DriveInfo.GetDrives();
}
