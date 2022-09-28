using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.View.Dashboard;
using Xperimen.View.Setting;
using SQLite;
using System.Linq;
using Rg.Plugins.Popup.Extensions;
using Xperimen.Stylekit;
using Xperimen.Helper;
using Xperimen.ViewModel.Setting;
using Xperimen.View.Commitment;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMenu : ContentPage
    {
        public ListView listview_menu;
        public SQLiteConnection connection;
        public Clients user_login;
        public StreamByteConverter converter;

        public DrawerMenu()
        {
            InitializeComponent();
            connection = new SQLiteConnection(App.DB_PATH);
            converter = new StreamByteConverter();
            listview_menu = listview;
            SetupData();
            CreateMenuList();

            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) => { SetupData(); });
        }

        public void SetupData()
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
                        var stream = converter.BytesToStream(user_login.ProfileImage);
                        return stream;
                    });
                    lbl_fullname.Text = login[0].Firstname + " " + login[0].Lastname;
                    lbl_name.Text = "@" + login[0].Username;
                    lbl_desc.Text = login[0].Description;
                }
            }
        }

        public void CreateMenuList()
        {
            List<ItemMenu> menulist = new List<ItemMenu>();

            menulist.Add(new ItemMenu { ImageIcon1 = "black_user.png", ImageIcon2 = "white_user.png", Title = "Dashboard", Contentpage = typeof(MainPage) });
            menulist.Add(new ItemMenu { ImageIcon1 = "black_whatshot.png", ImageIcon2 = "white_whatshot.png", Title = "Admin", Contentpage = typeof(AdminPage) });
            menulist.Add(new ItemMenu { ImageIcon1 = "black_whatshot.png", ImageIcon2 = "white_whatshot.png", Title = "Commitment", Contentpage = typeof(MainCommitment) });
            menulist.Add(new ItemMenu { ImageIcon1 = "black_money.png", ImageIcon2 = "white_money.png", Title = "Finance", Contentpage = typeof(MainPage) });
            menulist.Add(new ItemMenu { ImageIcon1 = "black_setting.png", ImageIcon2 = "white_setting.png", Title = "Setting", Contentpage = typeof(MainSetting) });
            menulist.Add(new ItemMenu { ImageIcon1 = "black_logout.png", ImageIcon2 = "white_logout.png", Title = "Logout", Contentpage = typeof(Logout) });
            listview.ItemsSource = menulist;
        }

        public async void OnHeaderTapped(object sender, EventArgs e)
        {
            await frame_profile.ScaleTo(0.9, 100);
            frame_profile.Scale = 1;
            await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(user_login.ProfileImage)));
        }
    }
}