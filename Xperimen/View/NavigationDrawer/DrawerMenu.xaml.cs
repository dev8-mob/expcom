using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.View.Dashboard;
using Xperimen.View.Setting;
using SQLite;
using System.Linq;
using System.IO;
using Rg.Plugins.Popup.Extensions;
using Xperimen.Stylekit;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMenu : ContentPage
    {
        public ListView listview_menu;
        public SQLiteConnection connection;
        public Clients user_login;

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
                    user_login = login[0];
                    img_pic.Source = ImageSource.FromStream(() =>
                    {
                        var stream = BytesToStream(user_login.ProfileImage);
                        return stream;
                    });
                    lbl_fullname.Text = login[0].Firstname + " " + login[0].Lastname;
                    lbl_name.Text = "@" + login[0].Username;
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
            await frame_profile.ScaleTo(0.9, 50);
            frame_profile.Scale = 1;
            await Navigation.PushPopupAsync(new ImageViewer(BytesToStream(user_login.ProfileImage)));
        }

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}