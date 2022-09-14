using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.View.Dashboard;
using Xperimen.View.Setting;
using SQLite;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMenu : ContentPage
    {
        public ListView listview_menu;
        public SQLiteConnection connection;

        public DrawerMenu()
        {
            InitializeComponent();
            connection = new SQLiteConnection(App.DB_PATH);
            listview_menu = listview;
            CreateMenuList();
        }

        public void CreateMenuList()
        {
            var login = connection.Table<ClientCurrent>().ToList();
            if (login.Count > 0)
            {
                lbl_name.Text = login[0].Username;
                lbl_desc.Text = login[0].Description;
            }
            
            List<ItemMenu> menulist = new List<ItemMenu>();

            menulist.Add(new ItemMenu { ImageIcon = "black_user.png", Title = "Dashboard", Contentpage = typeof(MainPage) });
            menulist.Add(new ItemMenu { ImageIcon = "black_whatshot.png", Title = "Tabbed Page", Contentpage = typeof(TabbedDashboard) });
            menulist.Add(new ItemMenu { ImageIcon = "black_password.png", Title = "Page Two", Contentpage = typeof(MainPage) });
            menulist.Add(new ItemMenu { ImageIcon = "black_setting.png", Title = "Setting", Contentpage = typeof(MainSetting) });
            menulist.Add(new ItemMenu { ImageIcon = "black_search.png", Title = "Logout", Contentpage = typeof(Logout) });
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