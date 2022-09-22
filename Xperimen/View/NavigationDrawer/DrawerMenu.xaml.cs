using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.View.Dashboard;
using Xperimen.View.Setting;
using SQLite;
using System.Linq;

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
            if (Application.Current.Properties.ContainsKey("current_login"))
            {
                var id = Application.Current.Properties["current_login"];
                var login = connection.Query<Clients>("SELECT * FROM Clients WHERE Id = '" + id + "'").ToList();
                if (login.Count > 0)
                {
                    lbl_name.Text = login[0].Username;
                    lbl_desc.Text = login[0].Description;
                }
            }
               
            List<ItemMenu> menulist = new List<ItemMenu>();

            menulist.Add(new ItemMenu { ImageIcon1 = "black_user.png", ImageIcon2 = "white_user.png", Title = "Dashboard", Contentpage = typeof(MainPage) });
            menulist.Add(new ItemMenu { ImageIcon1 = "black_whatshot.png", ImageIcon2 = "white_whatshot.png", Title = "Tabbed Page", Contentpage = typeof(TabbedDashboard) });
            menulist.Add(new ItemMenu { ImageIcon1 = "black_password.png", ImageIcon2 = "white_password.png", Title = "Page Two", Contentpage = typeof(MainPage) });
            menulist.Add(new ItemMenu { ImageIcon1 = "black_money.png", ImageIcon2 = "white_money.png", Title = "Finance", Contentpage = typeof(MainPage) });
            menulist.Add(new ItemMenu { ImageIcon1 = "black_setting.png", ImageIcon2 = "white_setting.png", Title = "Setting", Contentpage = typeof(MainSetting) });
            menulist.Add(new ItemMenu { ImageIcon1 = "black_logout.png", ImageIcon2 = "white_logout.png", Title = "Logout", Contentpage = typeof(Logout) });
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