
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.View.Dashboard;
using Xperimen.ViewModel.NavigationDrawer;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMaster : FlyoutPage
    {
        public DrawerViewmodel viewmodel;

        public DrawerMaster()
        {
            InitializeComponent();
            viewmodel = new DrawerViewmodel();
            BindingContext = viewmodel;
            Detail = new NavigationPage(new AdminPage());
        }
    }
}