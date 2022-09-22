using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.View.Dashboard;
using SQLite;

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

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"];
                var page = Application.Current.MainPage as NavigationPage;
                try
                {
                    if (theme.Equals("dark")) page.BarBackgroundColor = Color.Black;
                    if (theme.Equals("dim")) page.BarBackgroundColor = Color.SlateGray;
                    if (theme.Equals("light")) page.BarBackgroundColor = Color.Transparent;
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                }
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