
using SQLite;
using System.Linq;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.Stylekit
{
    public class MyLabel : Label
    {
        public SQLiteConnection Connection;

        public MyLabel()
        {
            Connection = new SQLiteConnection(App.DB_PATH);
            TextColor = (Color)Application.Current.Resources["LabelTextColor"];
            SetupView();
        }

        public void SetupView()
        {
            var login = Connection.Table<ClientCurrent>().ToList();
            if (login.Count > 0)
            {
                var query = "SELECT * FROM Clients WHERE Id = '" + login[0].UserId + "'";
                var result = Connection.Query<Clients>(query).ToList();
                if (result.Count > 0)
                {
                    if (result[0].AppTheme.Equals("dark")) TextColor = Color.White;
                    if (result[0].AppTheme.Equals("dim")) TextColor = Color.White;
                    if (result[0].AppTheme.Equals("light")) TextColor = (Color)Application.Current.Resources["LabelTextColor"];
                }
            }
        }
    }
}
