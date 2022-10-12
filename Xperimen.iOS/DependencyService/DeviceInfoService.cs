
using UIKit;
using Xamarin.Forms;
using Xperimen.Helper;
using Xperimen.iOS.DependencyService;

[assembly: Dependency(typeof(DeviceInfoService))]
namespace Xperimen.iOS.DependencyService
{
    public class DeviceInfoService : IDeviceInfo
    {
        public DeviceInfoService() { }

        public bool IsLowerIphoneDevice()
        {
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                if ((UIScreen.MainScreen.Bounds.Height / UIScreen.MainScreen.Bounds.Width) < 2)
                    return true;
            }
            return false;
        }
    }
}