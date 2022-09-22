
using SQLite;
using Xamarin.Forms;
using Xperimen.View;

namespace Xperimen.Stylekit
{
    public class XLabel : Label
    {
        public SQLiteConnection Connection;

        public XLabel()
        {
            Connection = new SQLiteConnection(App.DB_PATH);
            TextColor = (Color)Application.Current.Resources["LabelTextColor"];
            if (Device.RuntimePlatform == Device.Android) FontFamily = "Ubuntu-Regular.ttf#Ubuntu Regular";
            else if (Device.RuntimePlatform == Device.iOS) FontFamily = "Ubuntu-Regular.ttf";
            SetupView();

            MessagingCenter.Subscribe<CreateAccount>(this, "AppThemeUpdated", (sender) =>
            { SetupView(); });
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
