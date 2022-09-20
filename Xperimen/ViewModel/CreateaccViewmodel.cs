using System.Windows.Input;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel
{
    public class CreateaccViewmodel : BaseViewModel
    {
        #region properties
        string _username;
        string _password;
        string _datastring;
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
        public string DataString
        {
            get { return _datastring; }
            set { _datastring = value; OnPropertyChanged(); }
        }
        public bool Success
        {
            get { return _success; }
            set { _success = value; OnPropertyChanged(); }
        }
        #endregion
    }
}
