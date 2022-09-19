using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.View.Dashboard;
using SQLite;
using System.Linq;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMaster : MasterDetailPage
    {
        public ListView listview;
        public string Code;

        public DrawerMaster()
        {
            InitializeComponent();
            listview = drawerMenu.listview_menu;
            listview.ItemSelected += MenuSelected;
            var connection = new SQLiteConnection(App.DB_PATH);

            Detail = new NavigationPage(new TabbedDashboard());

            var current = connection.Table<ClientCurrent>().ToList();
            string query = "SELECT * FROM Clients WHERE Id = '" + current[0].UserId + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0)
            {
                var page = Application.Current.MainPage as NavigationPage;
                if (result[0].AppTheme.Equals("dark")) page.BarBackgroundColor = Color.Black;
                if (result[0].AppTheme.Equals("dim")) page.BarBackgroundColor = Color.FromHex(App.DimGray1);
                if (result[0].AppTheme.Equals("light")) page.BarBackgroundColor = Color.White;
            }
        }

        private void MenuSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var item = (ItemMenu)e.SelectedItem;
                Type page = item.Contentpage;
                var openPage = (Page)Activator.CreateInstance(page);
                Detail = new NavigationPage(openPage);

                #region old code
                //Detail = new NavigationPage(openPage);
                //if (title.Equals("My Profile")) Navigation.PushAsync(new Profile.Index());
                //else if (title.Equals("Reset Password")) Navigation.PushAsync(new ResetPassword.Index());
                //else Detail = new NavigationPage((Page)Activator.CreateInstance(page));
                //if (item.PageTitle.Equals("Home")) Detail = new NavigationPage(openPage);
                //else Navigation.PushAsync(openPage);
                #endregion

                IsPresented = false;
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}