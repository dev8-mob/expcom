
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xperimen.Helper;

namespace Xperimen.ViewModel
{
    public class LoginViewmodel : BaseViewModel, INotifyPropertyChanged
    {
        string _purchaseRequisitions;
        public string PurchaseRequisitions
        {
            get { return _purchaseRequisitions; }
            set { _purchaseRequisitions = value; OnPropertyChanged(); }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        public new void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ICommand StartTest { get; }

        public LoginViewmodel()
        {
            StartTest = new Command(SetView);
            PurchaseRequisitions = "First time assign text";
        }

        public async void SetView()
        {
            await Task.Delay(2000);
            PurchaseRequisitions = "Second time assign text !!";
        }
    }
}
