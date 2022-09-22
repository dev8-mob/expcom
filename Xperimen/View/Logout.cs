
using SQLite;
using Xamarin.Forms;

namespace Xperimen.View
{
    public class Logout : ContentPage
    {
        public SQLiteConnection connection;

        public Logout()
        {
            connection = new SQLiteConnection(App.DB_PATH);
            Application.Current.Properties.Remove("current_login");
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
