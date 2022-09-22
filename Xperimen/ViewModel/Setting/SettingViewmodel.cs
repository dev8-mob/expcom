
using Xperimen.Model;
using SQLite;
using System.Linq;
using Xamarin.Forms;

namespace Xperimen.ViewModel.Setting
{
    public class SettingViewmodel : BaseViewModel
    {
        #region properties
        string _username;
        string _password;
        string _description;
        string _theme;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        public string Theme
        {
            get { return _theme; }
            set { _theme = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;

        public SettingViewmodel()
        {
            connection = new SQLiteConnection(App.DB_PATH);

            var userid = Application.Current.Properties["current_login"] as string;
            string query = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0)
            {
                Username = result[0].Username;
                Password = result[0].Password;
                Description = result[0].Description;
                Theme = result[0].AppTheme;
            }
        }
    }
}
