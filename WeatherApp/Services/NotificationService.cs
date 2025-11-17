using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Services
{
    public class NotificationService
    {
        public NotificationService() { }

        /// <summary>
        /// Checks platform if is Android and if notification permission is granted
        /// </summary>
        /// <returns>If notification si enable returns true</returns>
        public async Task<bool> IsNotificationEnabledAsync()
        {
            if (DeviceInfo.Platform != DevicePlatform.Android)
                return false;

            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

            return status == PermissionStatus.Granted;
        }
    }
}
