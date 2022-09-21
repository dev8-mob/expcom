
using SQLite;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.View
{
    public class Logout : ContentPage
    {
        public SQLiteConnection connection;

        public Logout()
        {
            connection = new SQLiteConnection(App.DB_PATH);
            connection.DeleteAll<Clients>();
            Application.Current.Properties.Remove("current_login");
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
