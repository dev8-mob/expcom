
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel.Dashboard;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        //private readonly DetailspageViewmodel viewmodel;

        public DetailsPage()
        {
            InitializeComponent();
            BindingContext = new DetailspageViewmodel();
        }
    }
}