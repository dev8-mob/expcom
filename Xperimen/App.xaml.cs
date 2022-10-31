
using SQLite;
using Xamarin.Forms;
using Xperimen.Model;
using Xperimen.View;
using Xperimen.View.NavigationDrawer;

namespace Xperimen
{
    public partial class App : Application
    {
        public static string AppVersion = "1.7.1";
        public static string DB_PATH = string.Empty;
        public static string Primary = "#db5e0a";
        public static string PrimaryLight = "#f79655";
        public static string DimGray1 = "#F2F2F2";
        public static string DimGray2 = "#e7e7e7";
        public static string CustomGreen = "#039603";
        public static string CustomGreenLight = "#9cfc9c";
        public static string CustomRed = "#cb0101";
        public static string CustomRedLight = "#fe8080";
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
            connection.CreateTable<Expenses>();

            var clients = connection.Table<Clients>().ToList();
            var cek2 = connection.Table<SelfCommitment>().ToList();
            var cek3 = connection.Table<Expenses>().ToList();

            if (clients.Count > 0) 
            {
                if (Current.Properties.ContainsKey("current_login")) 
                    Current.MainPage = new NavigationPage(new DrawerMaster());
                else MainPage = new NavigationPage(new Login());
            }
            else MainPage = new NavigationPage(new Intro());
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
