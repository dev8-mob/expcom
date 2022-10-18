
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xperimen.Model;
using Xperimen.View;
using Xperimen.View.Commitment;
using Xperimen.View.Dashboard;
using Xperimen.View.Expense;
using Xperimen.View.Gallery;
using Xperimen.View.Setting;

namespace Xperimen.ViewModel.NavigationDrawer
{
    public class DrawerViewmodel : BaseViewModel
    {
        #region properties
        string _username;
        string _firstname;
        string _lastname;
        string _description;
        byte[] _picture;
        ObservableCollection<ItemMenu> _menulist;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; OnPropertyChanged(); }
        }
        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; OnPropertyChanged(); }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ItemMenu> MenuList
        {
            get { return _menulist; }
            set { _menulist = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;

        public DrawerViewmodel()
        {
            Username = string.Empty;
            Firstname = string.Empty;
            Lastname = string.Empty;
            Description = string.Empty;
            Picture = null;
            connection = new SQLiteConnection(App.DB_PATH);
            SetupData();
            SetupDataProfile();
            SetSelectedMenu("Admin");
        }

        public void SetupData()
        {
            var textcolor = (Color)Application.Current.Resources["LabelTextColor"];
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) textcolor = Color.White;
                if (theme.Equals("dim")) textcolor = Color.Black;
                if (theme.Equals("light")) textcolor = (Color)Application.Current.Resources["LabelTextColor"];
            }

            #region set menu list
            MenuList = new ObservableCollection<ItemMenu>();
            MenuList.Add(new ItemMenu
            {
                ImageIcon1 = "black_user.png",
                ImageIcon2 = "white_user.png",
                Title = "Dashboard",
                Contentpage = typeof(Welcome),
                IsSelected = false,
                TextMenuColor = textcolor
            });
            MenuList.Add(new ItemMenu
            {
                ImageIcon1 = "black_wallet.png",
                ImageIcon2 = "white_wallet.png",
                Title = "Commitment",
                Contentpage = typeof(MainCommitment),
                IsSelected = false,
                TextMenuColor = textcolor
            });
            MenuList.Add(new ItemMenu
            {
                ImageIcon1 = "black_money.png",
                ImageIcon2 = "white_money.png",
                Title = "Expenses",
                Contentpage = typeof(MainExpenses),
                IsSelected = false,
                TextMenuColor = textcolor
            });
            MenuList.Add(new ItemMenu
            {
                ImageIcon1 = "black_image.png",
                ImageIcon2 = "white_image.png",
                Title = "Gallery",
                Contentpage = typeof(MainGallery),
                IsSelected = false,
                TextMenuColor = textcolor
            });
            MenuList.Add(new ItemMenu
            {
                ImageIcon1 = "black_setting.png",
                ImageIcon2 = "white_setting.png",
                Title = "Settings",
                Contentpage = typeof(MainSetting),
                IsSelected = false,
                TextMenuColor = textcolor
            });
            MenuList.Add(new ItemMenu
            {
                ImageIcon1 = "black_logout.png",
                ImageIcon2 = "white_logout.png",
                Title = "Logout",
                Contentpage = typeof(Logout),
                IsSelected = false,
                TextMenuColor = textcolor
            });
            #endregion
        }

        public void SetupDataProfile()
        {
            if (Application.Current.Properties.ContainsKey("current_login"))
            {
                var id = Application.Current.Properties["current_login"];
                var login = connection.Query<Clients>("SELECT * FROM Clients WHERE Id = '" + id + "'").ToList();
                if (login.Count > 0)
                {
                    Username = login[0].Username;
                    Firstname = login[0].Firstname;
                    Lastname = login[0].Lastname;
                    Description = login[0].Description;
                    Picture = login[0].ProfileImage;
                }
            }
        }

        public void SetSelectedMenu(string title)
        {
            string theme = string.Empty;
            if (Application.Current.Properties.ContainsKey("app_theme"))
                theme = Application.Current.Properties["app_theme"] as string;

            if (MenuList.Count > 0)
            {
                foreach (var data in MenuList)
                {
                    if (data.Title.Equals(title))
                    {
                        data.IsSelected = true;
                        if (!string.IsNullOrEmpty(theme))
                        {
                            //set here and DrawerMenuCell messagingcenter
                            data.TextMenuColor = Color.White;
                        }
                    }
                }
            }
        }
    }
}
