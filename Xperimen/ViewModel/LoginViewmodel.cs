
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel
{
    public class LoginViewmodel : BaseViewModel, INotifyPropertyChanged
    {
        string _logo;
        public string Logo
        {
            get { return _logo; }
            set { _logo = value; OnPropertyChanged(); }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        public new void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

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
