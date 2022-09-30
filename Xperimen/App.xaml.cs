
using SQLite;
using Xamarin.Forms;
using Xperimen.Model;
using Xperimen.View;
using Xperimen.View.NavigationDrawer;

namespace Xperimen
{
    public partial class App : Application
    {
        public static string DB_PATH = string.Empty;
        public static string Primary = "#f46a11";
        public static string DimGray1 = "#F2F2F2";
        public static string DimGray2 = "#e7e7e7";
        public static string CustomGreen = "#04B404";
        public static string CustomRed = "#DF0101";
        public static string CustomYellow = "#DEAF14";
        public static string CustomBlue = "#01A9DB";
        public static string LabelTextColor = "#666666";
        public static string SlateGray = "#708090";
        public static string CharcoalGray = "#8c99a6";
        public static string CharcoalBlack = "#2E2E2E";

        public App(string sql_path)
        {
            InitializeComponent();
            DB_PATH = sql_path;
            var connection = new SQLiteConnection(DB_PATH);
            connection.CreateTable<Clients>();
            connection.CreateTable<SelfCommitment>();

            if (Current.Properties.ContainsKey("current_login")) Current.MainPage = new NavigationPage(new DrawerMaster());
            else MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
