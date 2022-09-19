
using SQLite;
using System.Linq;
using Xamarin.Forms;
using Xperimen.Model;

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
                    if (result[0].AppTheme.Equals("dim")) BackgroundColor = Color.SlateGray;
                    if (result[0].AppTheme.Equals("light")) BackgroundColor = Color.Transparent;
                }
            }
        }
    }
}
