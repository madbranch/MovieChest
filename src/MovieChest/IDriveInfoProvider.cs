using System.IO;

namespace MovieChest;

public interface IDriveInfoProvider
{
    DriveInfo[] GetDrives(); 
}
