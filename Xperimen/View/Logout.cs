
using SQLite;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.View
{
    public class Logout : ContentPage
    {
        public Logout()
        {
            var userid = Application.Current.Properties["current_login"] as string;
            var connection = new SQLiteConnection(App.DB_PATH);
            var queryLogout = "UPDATE Clients SET IsLogin = false WHERE Id = '" + userid + "'";
            connection.Query<Clients>(queryLogout);
            Application.Current.Properties.Remove("current_login");
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
