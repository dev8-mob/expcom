using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Model;
using Xperimen.Stylekit;
using Xperimen.View.Commitment;
using Xperimen.ViewModel.NavigationDrawer;
using Xperimen.ViewModel.Setting;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMenu : ContentPage
    {
        public DrawerViewmodel viewmodel;
        public Clients user_login;
        public StreamByteConverter converter;

        public DrawerMenu()
        {
            InitializeComponent();
            converter = new StreamByteConverter();
            viewmodel = new DrawerViewmodel();
            BindingContext = viewmodel;
            SetupView();

            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) => { UpdateDataProfile(); });
        }

        public void SetupView()
        {
            // setup for different iphone screen sizes
            if (Device.RuntimePlatform == Device.iOS)
            {
                var lowerscreen = DependencyService.Get<IDeviceInfo>().IsLowerIphoneDevice();
                if (lowerscreen)
                {
                    var safeInsets = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();
                    safeInsets.Top = -20;
                    Padding = safeInsets;
                }
            }
            UpdateDataProfile();
        }

        public void UpdateDataProfile()
        {
            viewmodel.SetupDataProfile();
            if (viewmodel.Picture != null)
            {
                img_pic.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(viewmodel.Picture);
                    return stream;
                });
            }
        }

        public async void OnHeaderTapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await frame_profile.ScaleTo(0.9, 100);
            frame_profile.Scale = 1;
            view.IsEnabled = false;

            if (viewmodel.Picture != null)
                await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(viewmodel.Picture)));
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
                var openPage = (Xamarin.Forms.Page)Activator.CreateInstance(page);
                viewmodel.SetSelectedMenu(title);
                parent.IsPresented = false;
                parent.Detail = new Xamarin.Forms.NavigationPage(openPage);
            }
        }

        private void listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var view = (Xamarin.Forms.ListView)sender;
            view.IsEnabled = false;
            if (e.SelectedItem != null) view.SelectedItem = null;
            view.IsEnabled = true;
        }
    }
}