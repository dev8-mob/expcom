
using SQLite;
using System.Linq;
using Xamarin.Forms;
using Xperimen.Model;
using Xperimen.View;
using Xperimen.View.Setting;

namespace Xperimen.Stylekit
{
    public class MyStackLayout : StackLayout
    {
        public SQLiteConnection Connection;

        public MyStackLayout()
        {
            Connection = new SQLiteConnection(App.DB_PATH);
            BackgroundColor = Color.White;
            MessagingCenter.Subscribe<MainSetting>(this, "SelectedTheme", s => { SetupView(); });
            MessagingCenter.Subscribe<CreateAccount>(this, "SelectedTheme", s => { SetupView(); });
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
                    if (result[0].AppTheme.Equals("dark")) BackgroundColor = Color.Black;
                    if (result[0].AppTheme.Equals("light")) BackgroundColor = Color.White;
                }
            }
        }
    }
}
