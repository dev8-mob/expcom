
using Xperimen.Model;
using Xperimen.View.Commitment;
using Xperimen.View.Dashboard;
using Xperimen.View.Expense;
using Xperimen.View.Setting;
using Xperimen.View;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Xperimen.ViewModel.NavigationDrawer
{
    public class DrawerViewmodel : BaseViewModel
    {
        ObservableCollection<ItemMenu> _menulist;
        public ObservableCollection<ItemMenu> MenuList
        {
            get { return _menulist; }
            set { _menulist = value; OnPropertyChanged(); }
        }

        public DrawerViewmodel()
        { 
            SetupData();
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
                Contentpage = typeof(MainPage), 
                IsSelected = false,
                TextMenuColor = textcolor
            });
            MenuList.Add(new ItemMenu 
            { 
                ImageIcon1 = "black_whatshot.png", 
                ImageIcon2 = "white_whatshot.png", 
                Title = "Admin", 
                Contentpage = typeof(AdminPage), 
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
                ImageIcon1 = "black_setting.png", 
                ImageIcon2 = "white_setting.png", 
                Title = "Setting", 
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
