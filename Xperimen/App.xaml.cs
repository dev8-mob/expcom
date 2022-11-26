
using SQLite;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xamarin.Forms;
using Xperimen.Model;
using Xperimen.Resources;
using Xperimen.View;
using Xperimen.View.NavigationDrawer;

namespace Xperimen
{
    public partial class App : Application
    {
        public static string AppVersion = "1.7.5";
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

            CleanData();
            var clients = connection.Table<Clients>().ToList();
            if (clients.Count > 0)
            {
                if (Current.Properties.ContainsKey("current_login"))
                {
                    var lang = string.Empty;
                    var userid = Current.Properties["current_login"] as string;
                    string query = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
                    var result = connection.Query<Clients>(query).ToList();
                    if (result.Count > 0) lang = result[0].Language;
                    CultureInfo language = new CultureInfo(lang);
                    Thread.CurrentThread.CurrentUICulture = language;
                    AppResources.Culture = language;
                    Current.MainPage = new NavigationPage(new DrawerMaster("welcome"));
                }
                else MainPage = new NavigationPage(new Login());
            }
            else
            {
                Current.Properties.Remove("app_theme");
                Current.SavePropertiesAsync();
                MainPage = new NavigationPage(new Intro());
            }
        }

        public void CleanData()
        {
            var connection = new SQLiteConnection(DB_PATH);
            var listcommit = connection.Table<SelfCommitment>().ToList();
            var listexp = connection.Table<Expenses>().ToList();

            if (listcommit.Count > 0)
            {
                foreach (var item in listcommit)
                {
                    if (string.IsNullOrEmpty(item.Currency))
                    {
                        var query = "UPDATE SelfCommitment SET Currency = 'RM' WHERE Id = '" + item.Id + "'";
                        connection.Query<SelfCommitment>(query);
                    }
                }
            }
            if (listexp.Count > 0)
            {
                foreach (var item in listexp)
                {
                    if (string.IsNullOrEmpty(item.Currency))
                    {
                        var query = "UPDATE Expenses SET Currency = 'RM' WHERE Id = '" + item.Id + "'";
                        connection.Query<Expenses>(query);
                    }
                }
            }
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
