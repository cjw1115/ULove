using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.UserProfile;

namespace SetBackgroundLib
{
    public class BackgroundService
    {
        public static bool IsSupportSetBackground()
        {
            return UserProfilePersonalizationSettings.IsSupported();
        }
        public async static Task<bool> SetBackground(StorageFile backgroundFile)
        {
            var current = UserProfilePersonalizationSettings.Current;
            if (current != null)
            {
                return await current.TrySetWallpaperImageAsync(backgroundFile);
            }
            return false;
        }
    }
}
