
using SQLite;
using Xamarin.Forms;
using Xperimen.Model;
using Xperimen.View.Setting;

namespace Xperimen.Stylekit
{
    public class MyLabel : Label
    {
        public SQLiteConnection Connection;

        public MyLabel()
        {
            Connection = new SQLiteConnection(App.DB_PATH);
            var loginBool = Connection.Table<Clients>().ToList();

            TextColor = (Color)Application.Current.Resources["LabelTextColor"];
            MessagingCenter.Subscribe<MainSetting, string>(this, "SelectedTheme", (s, e) => {
                if (e.Equals("dark")) TextColor = Color.White;
                if (e.Equals("light")) TextColor = (Color)Application.Current.Resources["LabelTextColor"];
            });
        }
    }
}
