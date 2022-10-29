
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.View.Dashboard;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMaster : FlyoutPage
    {
        public SQLiteConnection connection;

        public DrawerMaster()
        {
            InitializeComponent();
            Detail = new NavigationPage(new Welcome());
        }
    }
}