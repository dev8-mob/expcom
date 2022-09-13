using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.View.Dashboard;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Obsolete]
    public partial class DrawerMaster : MasterDetailPage
    {
        public ListView listview;
        public string Code;

        public DrawerMaster()
        {
            InitializeComponent();
            listview = drawerMenu.listview_menu;
            listview.ItemSelected += MenuSelected;

            Detail = new NavigationPage(new MainPage());
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