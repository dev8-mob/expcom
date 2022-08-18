
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel.Dashboard;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainpageViewmodel();
        }
    }
}