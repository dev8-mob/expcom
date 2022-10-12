using Xamarin.Forms;
using Xperimen.ViewModel;
using Xperimen.ViewModel.Setting;

namespace Xperimen.Stylekit
{
    public class XLabel : Label
    {
        public XLabel()
        {
            TextColor = (Color)Application.Current.Resources["LabelTextColor"];
            if (Device.RuntimePlatform == Device.Android) FontFamily = "Ubuntu-Regular.ttf#Ubuntu Regular";
            else if (Device.RuntimePlatform == Device.iOS) FontFamily = "Ubuntu-Regular";
            SetupView();

            MessagingCenter.Subscribe<CreateaccViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<LoginViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) TextColor = Color.White;
                if (theme.Equals("dim")) TextColor = Color.Black;
                if (theme.Equals("light")) TextColor = (Color)Application.Current.Resources["LabelTextColor"];
            }
            else TextColor = (Color)Application.Current.Resources["LabelTextColor"];
        }
    }
}
