
using System.Windows.Input;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel
{
    public class LoginViewmodel : BaseViewModel
    {
        #region properties
        string _logo;
        public string Logo
        {
            get { return _logo; }
            set { _logo = value; OnPropertyChanged(); }
        }
        string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        #endregion

        public ICommand StartTest { get; }

        public LoginViewmodel()
        {
            StartTest = new Command(ChangeView);
            Logo = "black_whatshot.png";
        }

        public void ChangeView()
        {
            if (Logo.Equals("black_whatshot.png")) Logo = "white_whatshot.png";
            else if (Logo.Equals("white_whatshot.png")) Logo = "black_whatshot.png";
        }
    }
}
