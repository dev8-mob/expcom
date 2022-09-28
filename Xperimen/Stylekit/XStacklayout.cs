
using Xamarin.Forms;
using Xperimen.ViewModel;
using Xperimen.ViewModel.Setting;

namespace Xperimen.Stylekit
{
    public class XStacklayout : StackLayout
    {
        public XStacklayout()
        {
            BackgroundColor = Color.Transparent;
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
                if (theme.Equals("dark")) BackgroundColor = Color.Black;
                if (theme.Equals("dim")) BackgroundColor = Color.SlateGray;
                if (theme.Equals("light")) BackgroundColor = Color.FromHex(App.DimGray1);
            }
            else BackgroundColor = Color.Transparent;
        }
    }
}
