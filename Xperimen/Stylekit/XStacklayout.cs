
using SQLite;
using Xamarin.Forms;
using Xperimen.ViewModel;

namespace Xperimen.Stylekit
{
    public class XStacklayout : StackLayout
    {
        public SQLiteConnection Connection;

        public XStacklayout()
        {
            Connection = new SQLiteConnection(App.DB_PATH);
            BackgroundColor = Color.Transparent;
            SetupView();

            MessagingCenter.Subscribe<CreateaccViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<LoginViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) BackgroundColor = Color.Black;
                if (theme.Equals("dim")) BackgroundColor = Color.SlateGray;
                if (theme.Equals("light")) BackgroundColor = Color.Transparent;
            }
            else BackgroundColor = Color.Transparent;
        }
    }
}
