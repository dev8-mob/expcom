using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Stylekit;
using Xperimen.View.NavigationDrawer;
using Xperimen.ViewModel.Setting;

namespace Xperimen.View.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSetting : ContentPage
    {
        public SettingViewmodel viewmodel;
        public StreamByteConverter converter;

        public MainSetting()
        {
            InitializeComponent();
            viewmodel = new SettingViewmodel();
            converter = new StreamByteConverter();
            view_setting.SetParentBinding(viewmodel);
            BindingContext = viewmodel;
            SetupView();

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) => 
            { 
                viewmodel.IsLoading = false;
                if (alert.CodeObject.Equals("reset"))
                    Xamarin.Forms.Application.Current.MainPage = new Xamarin.Forms.NavigationPage(new Login());
            });
            MessagingCenter.Subscribe<SettingInfo>(this, "SettingEditProfile", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<AccountList>(this, "deleteme", (sender) => 
            {
                viewmodel.IsLoading = true;
                SetDisplayAlert("Success", "Account and all the data successfully deleted.", "", "", "reset"); 
            });
        }

        public void SetupView()
        {
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Xamarin.Forms.Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) img_profile.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) img_profile.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) img_profile.BackgroundColor = Color.FromHex(App.DimGray2);
            }
            else img_profile.BackgroundColor = Color.FromHex(App.DimGray2);

            img_profile.Source = string.Empty;
            if (viewmodel.Picture != null)
            {
                img_profile.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(viewmodel.Picture);
                    return stream;
                });
            }

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
        }

        public async void ProfilePicClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            if (viewmodel.Picture != null)
            {
                view.IsEnabled = false;
                await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(viewmodel.Picture)));
                view.IsEnabled = true;
            }
        }

        public async void PicClearClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            viewmodel.Picture = null;
            SetupView();
            view.IsEnabled = true;
        }

        public async void GalleryClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            var result = await viewmodel.PickPhoto();
            if (result == 3) SetDisplayAlert("Unavailable", "Photo gallery is not available to pick photo.", "", "", "");
            else if (result == 2)
            {
                img_profile.Source = "";
                SetDisplayAlert("Alert", "No photo selected.", "", "", "");
            }
            else if (result == 1)
            {
                img_profile.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(viewmodel.Picture);
                    return stream;
                });
                viewmodel.IsLoading = false;
            }
            view.IsEnabled = true;
        }

        public async void CameraClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            var result = await viewmodel.TakePhoto();
            if (result == 3) SetDisplayAlert("Unavailable", "Camera is not available or take photo not supported.", "", "", "");
            else if (result == 2)
            {
                img_profile.Source = "";
                SetDisplayAlert("Alert", "Take photo cancelled.", "", "", "");
            }
            else if (result == 1)
            {
                img_profile.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(viewmodel.Picture);
                    return stream;
                });
                viewmodel.IsLoading = false;
            }
            view.IsEnabled = true;
        }

        public async void AppThemeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var stack = (StackLayout)view.Content;
            var lbltheme = (Label)stack.Children[1];

            if (lbltheme.Text.Equals("Dark")) { viewmodel.Theme = "dark"; }
            else if (lbltheme.Text.Equals("Dim")) { viewmodel.Theme = "dim"; }
            else if (lbltheme.Text.Equals("Light")) { viewmodel.Theme = "light"; }

            viewmodel.IsLoading = true;
            var result = await viewmodel.UpdateAppTheme();
            if (result == 1)
            {
                viewmodel.IsLoading = false;
                SetupView();
            }
            if (result == 2) SetDisplayAlert("Error", "Update application theme failed.", "", "", "");
            view.IsEnabled = true;
        }

        public async void SaveClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            //if (viewmodel.Picture == null) SetDisplayAlert("Alert", "Profile picture is empty. Please take a photo or choose a picture.", "", "", "");
            if (string.IsNullOrEmpty(viewmodel.Firstname)) SetDisplayAlert("Alert", "First name cannot be empty. Please insert your first name.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Lastname)) SetDisplayAlert("Alert", "Last name cannot be empty. Please insert your last name.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Username)) SetDisplayAlert("Alert", "Username cannot be empty. Please insert your username.", "", "", "");
            else if (viewmodel.Username.Length < 6) SetDisplayAlert("Alert", "Username must be more than 6 characters.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Password)) SetDisplayAlert("Alert", "Password cannot be empty. Please insert your password.", "", "", "");
            else if (viewmodel.Password.Length < 6) SetDisplayAlert("Alert", "Password must be more than 6 characters.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Repassword)) SetDisplayAlert("Confirmation Password", "Please re-type your password.", "", "", "");
            else if (!viewmodel.Repassword.Equals(viewmodel.Password))
            {
                SetDisplayAlert("Not Match", "Confirmation password is not match with current password.", "", "", "");
                viewmodel.Repassword = string.Empty;
            }
            else
            {
                var result = viewmodel.UpdateSetting();
                if (result == 1)
                {
                    SetDisplayAlert("Success", "Profile updated.", "", "", "");
                    viewmodel.IsViewing = true;
                    viewmodel.IsEditing = false;
                }
                else if (result == 2) SetDisplayAlert("Error", "Current login information not found.", "", "", "");
                else if (result == 3) SetDisplayAlert("Error", "Update information failed.", "", "", "");
                else if (result == 4) SetDisplayAlert("Exist", "A user with that username already exist. Please choose a different username.", "", "", "");
            }
            view.IsEnabled = true;
        }

        public async void CancelClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            viewmodel.Picture = null;
            viewmodel.SetupData();
            img_profile.SetupView();
            viewmodel.IsViewing = true;
            viewmodel.IsEditing = false;
            view.IsEnabled = true;
        }

        public async void DrawerTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var drawer = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
            drawer.IsPresented = true;
            view.IsEnabled = true;
        }

        public async void TopSettingTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushPopupAsync(new AccountList());
            view.IsEnabled = true;
        }

        public void SetDisplayAlert(string title, string description, string btn1, string btn2, string obj)
        {
            //if string1 empty will not display btn1, if string2 empty will not display btn2
            //if both string1 & string2 empty will not display all buttons
            //all buttons tapped will send 'DisplayAlertSelection' with text of the button
            //close button tapped will send 'DisplayAlertSelection' with empty text
            alert.Title = title;
            alert.Description = description;
            alert.TxtBtn1 = btn1;
            alert.TxtBtn2 = btn2;
            alert.IsVisible = true;
            alert.CodeObject = obj;
        }
    }
}