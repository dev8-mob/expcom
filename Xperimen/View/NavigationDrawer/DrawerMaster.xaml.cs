
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.View.Dashboard;
using Xperimen.View.Setting;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMaster : FlyoutPage
    {
        public SQLiteConnection connection;

        public DrawerMaster(string openpage)
        {
            InitializeComponent();

            if (openpage.Equals("welcome")) Detail = new NavigationPage(new Welcome());
            else if (openpage.Equals("setting")) Detail = new NavigationPage(new MainSetting());
        }
    }
}