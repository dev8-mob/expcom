﻿
using System.Windows.Input;
using Xperimen.Model;
using SQLite;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;

namespace Xperimen.ViewModel
{
    public class LoginViewmodel : BaseViewModel
    {
        #region properties
        string _username;
        string _password;

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
        #endregion

        public ICommand DoLogin { get; set; }
        public SQLiteConnection connection;

        public LoginViewmodel()
        {
            Username = string.Empty;
            Password = string.Empty;
            connection = new SQLiteConnection(App.DB_PATH);
        }

        public async Task<int> Login()
        {
            string query = "SELECT * FROM Clients WHERE Username = '" + Username + "' AND Password = '" + Password + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0)
            {
                Application.Current.Properties["current_login"] = result[0].Id;
                await Application.Current.SavePropertiesAsync();
                return 1;
            }
            else
            {
                query = "SELECT * FROM Clients WHERE Username = '" + Username + "'";
                result = connection.Query<Clients>(query).ToList();
                if (result.Count > 0)
                {
                    Password = string.Empty;
                    return 2;
                }
                else return 3;
            }
        }
    }
}
