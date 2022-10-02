using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.View.Dashboard;

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

            Detail = new NavigationPage(new AdminPage());
        }

        private async void MenuSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var view = (ListView)sender;
            view.IsEnabled = false;
            if (e.SelectedItem != null)
            {
                var item = (ItemMenu)e.SelectedItem;
                Type page = item.Contentpage;
                var openPage = (Page)Activator.CreateInstance(page);

                IsPresented = false;
                if (item.Title.Equals("Dashboard")) Detail = new NavigationPage(openPage);
                else await Navigation.PushAsync(openPage);
                ((ListView)sender).SelectedItem = null;

                #region old code
                //Detail = new NavigationPage(openPage);
                //if (title.Equals("My Profile")) Navigation.PushAsync(new Profile.Index());
                //else if (title.Equals("Reset Password")) Navigation.PushAsync(new ResetPassword.Index());
                //else Detail = new NavigationPage((Page)Activator.CreateInstance(page));
                //if (item.PageTitle.Equals("Home")) Detail = new NavigationPage(openPage);
                //else Navigation.PushAsync(openPage);
                #endregion
            }
            view.IsEnabled = true;
        }
    }
}