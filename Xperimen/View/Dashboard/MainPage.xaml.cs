
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel.Dashboard;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly MainpageViewmodel viewmodel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewmodel = new MainpageViewmodel();
        }
    }
}