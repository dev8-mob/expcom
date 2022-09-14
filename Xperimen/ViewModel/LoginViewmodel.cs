
using System.Windows.Input;
using Xperimen.Model;
using SQLite;

namespace Xperimen.ViewModel
{
    public class LoginViewmodel : BaseViewModel
    {
        #region properties
        string _username;
        string _password;
        bool _success;

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
        public bool Success
        {
            get { return _success; }
            set { _success = value; OnPropertyChanged(); }
        }
        #endregion

        public ICommand DoLogin { get; set; }
        public SQLiteConnection connection;

        public LoginViewmodel()
        {
            //DoLogin = new Command(Login);
            connection = new SQLiteConnection(App.DB_PATH);
            Success = false;
        }
    }
}
