
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
        string _firstname;
        string _lastname;
        string _password;
        string _repassword;
        string _description;
        string _theme;
        bool _isediting;
        bool _isviewing;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; OnPropertyChanged(); }
        }
        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; OnPropertyChanged(); }
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
        public bool IsEditing
        {
            get { return _isediting; }
            set { _isediting = value; OnPropertyChanged(); }
        }
        public bool IsViewing
        {
            get { return _isviewing; }
            set { _isviewing = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;
        public Clients user_login;

        public SettingViewmodel()
        {
            connection = new SQLiteConnection(App.DB_PATH);
            IsViewing = true;
            IsEditing = true;

            var userid = Application.Current.Properties["current_login"] as string;
            string query = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0)
            {
                user_login = result[0];
                Username = result[0].Username;
                Firstname = result[0].Firstname;
                Lastname = result[0].Lastname;
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
