
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.View.Dashboard;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMaster : FlyoutPage
    {
        public DrawerMaster()
        {
            InitializeComponent();
            Detail = new Welcome();
        }
    }
}