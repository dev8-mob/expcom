
using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.Stylekit
{
    public class XLabel : Label
    {
        public SQLiteConnection Connection;

        public XLabel()
        {
            Connection = new SQLiteConnection(App.DB_PATH);
            //if (Device.RuntimePlatform == Device.iOS) 
            //    FontFamily = "Ubuntu-Regular";
            //else if (Device.RuntimePlatform == Device.Android) 
            //    FontFamily = "Ubuntu-Regular.ttf#Ubuntu Regular";
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
                    if (result[0].AppTheme.Equals("dark")) TextColor = (Color)Application.Current.Resources["LabelTextColor"];
                    if (result[0].AppTheme.Equals("dim")) TextColor = Color.Black;
                    if (result[0].AppTheme.Equals("light")) TextColor = (Color)Application.Current.Resources["LabelTextColor"];
                }
            }
        }
    }
}
