
using Xperimen.Model;
using SQLite;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Xperimen.ViewModel.Setting
{
    public class SettingViewmodel : BaseViewModel
    {
        #region properties
        string _username;
        string _password;
        string _repassword;
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
        public string Repassword
        {
            get { return _repassword; }
            set { _repassword = value; OnPropertyChanged(); }
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

        public async Task<int> UpdateSetting()
        {
            Application.Current.Properties["app_theme"] = Theme;
            await Application.Current.SavePropertiesAsync();
            MessagingCenter.Send(this, "AppThemeUpdated");

            if (!Repassword.Equals(Password)) return 2;
            else return 1;
        }
    }
}
