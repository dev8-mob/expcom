
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
            connection.DeleteAll<ClientCurrent>();
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
