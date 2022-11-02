
using SQLite;
using Xamarin.Forms;
using Xperimen.Model;
using Xperimen.View;
using Xperimen.View.NavigationDrawer;

namespace Xperimen
{
    public partial class App : Application
    {
        public static string AppVersion = "1.7.4";
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
                    Current.MainPage = new NavigationPage(new DrawerMaster());
                else MainPage = new NavigationPage(new Login());
            }
            else MainPage = new NavigationPage(new Intro());
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
