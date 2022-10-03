using Rg.Plugins.Popup.Extensions;
using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Model;
using Xperimen.Stylekit;
using Xperimen.ViewModel.NavigationDrawer;
using Xperimen.ViewModel.Setting;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMenu : ContentPage
    {
        public DrawerViewmodel viewmodel;
        public SQLiteConnection connection;
        public Clients user_login;
        public StreamByteConverter converter;

        public DrawerMenu()
        {
            InitializeComponent();
            viewmodel = new DrawerViewmodel();
            BindingContext = viewmodel;
            connection = new SQLiteConnection(App.DB_PATH);
            converter = new StreamByteConverter();
            UpdateProfilePic();

            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) => 
            { UpdateProfilePic(); });
            MessagingCenter.Subscribe<DrawerMenuCell>(this, "DrawerMenuSelected", (sender) => 
            { listview.ItemsSource = viewmodel.MenuList; });
        }

        public void UpdateProfilePic()
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

        public async void OnHeaderTapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await frame_profile.ScaleTo(0.9, 100);
            frame_profile.Scale = 1;
            view.IsEnabled = false;

            await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(user_login.ProfileImage)));
            view.IsEnabled = true;
        }

        public async void CellTapped(object sender, EventArgs e)
        {
            var view = (Grid)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            var stack = (StackLayout)view.Children[1];
            var lbltitle = (Label)stack.Children[1];
            var title = lbltitle.Text;

            var parent = (DrawerMaster)Parent;
            var item = viewmodel.MenuList.Where(x => x.Title.Equals(title)).ToList();
            if (item.Count > 0)
            {
                Type page = item[0].Contentpage;
                var openPage = (Page)Activator.CreateInstance(page);
                viewmodel.SetupData(title);
                parent.IsPresented = false;
                parent.Detail = new NavigationPage(openPage);
            }
        }

        private void listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var view = (ListView)sender;
            view.IsEnabled = false;

            var parent = (DrawerMaster)view.Parent.Parent.Parent;
            if (e.SelectedItem != null)
            {
                var item = (ItemMenu)e.SelectedItem;
                Type page = item.Contentpage;
                var openPage = (Page)Activator.CreateInstance(page);

                parent.IsPresented = false;
                parent.Detail = new NavigationPage(openPage);
                ((ListView)sender).SelectedItem = null;
            }
            view.IsEnabled = true;
        }
    }
}