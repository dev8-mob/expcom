using SQLite;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel
{
    public class CreateaccViewmodel : BaseViewModel
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

        public CreateaccViewmodel()
        {
            Username = string.Empty;
            Password = string.Empty;
            Description = string.Empty;
            connection = new SQLiteConnection(App.DB_PATH);
        }

        public async Task<int> CreateAccount()
        {
            string query = "SELECT * FROM Clients WHERE Username = '" + Username + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0) return 1;
            else
            {
                var data = new Clients
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = Username,
                    Password = Password,
                    Description = Description,
                    AppTheme = Theme
                };
                connection.Insert(data);
                Application.Current.Properties["app_theme"] = Theme;
                await Application.Current.SavePropertiesAsync();
                MessagingCenter.Send(this, "AppThemeUpdated");
                Username = string.Empty;
                Password = string.Empty;
                Description = string.Empty;
                Theme = string.Empty;
                return 2;
            }
        }
    }
}
