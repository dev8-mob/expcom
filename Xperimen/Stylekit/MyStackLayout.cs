
using SQLite;
using Xamarin.Forms;
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
            MessagingCenter.Subscribe<MainSetting, string>(this, "SelectedTheme", (s, e) => {
                if (e.Equals("dark")) BackgroundColor = Color.Black;
                if (e.Equals("light")) BackgroundColor = Color.White;
            });
        }
    }
}
