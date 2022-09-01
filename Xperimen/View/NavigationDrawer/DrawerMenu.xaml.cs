using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.View.Dashboard;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMenu : ContentPage
    {
        public ListView listview_menu;

        public DrawerMenu()
        {
            InitializeComponent();
            listview_menu = listview;
            CreateMenuList();
        }

        public void CreateMenuList()
        {
            lbl_name.Text = "--";
            lbl_email.Text = "--";
            List<ItemMenu> menulist = new List<ItemMenu>();

            menulist.Add(new ItemMenu { ImageIcon = "black_user.png", Title = "Dashboard", Contentpage = typeof(TabbedDashboard) });
            menulist.Add(new ItemMenu { ImageIcon = "black_whatshot.png", Title = "Page One", Contentpage = typeof(MainPage) });
            menulist.Add(new ItemMenu { ImageIcon = "black_password.png", Title = "Page Two", Contentpage = typeof(MainPage) });
            listview.ItemsSource = menulist;
        }

        public async void OnHeaderTapped(object sender, EventArgs e)
        {
            await img_logo.ScaleTo(0.9, 50);
            img_logo.Scale = 1;
            //await Navigation.PushPopupAsync(new PopupViewImage(FileMedia, image_profile.Source.ToString()));
        }
    }
}