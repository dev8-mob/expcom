using Rg.Plugins.Popup.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Resources;
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

            #region messaging center
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
                SetDisplayAlert(AppResources.app_success, AppResources.code_setting_allsuccessdelete, "", "", "reset"); 
            });
            MessagingCenter.Subscribe<CurrencyList, string>(this, "CurrencyUpdated", (sender, arg) => 
            {
                var split = arg.Split(',');
                if (split.Count() > 0)
                {
                    lbl_currency.Text = split[1] + " - " + split[2];
                    var success = viewmodel.UpdateExpenseCurrency(split[0]);
                    if (success == 1)
                    {
                        success = viewmodel.UpdateCommitmentCurrency(split[0]);
                        if (success == 1) viewmodel.SetupData();
                    }
                }
            });
            MessagingCenter.Subscribe<LanguageList, string>(this, "LanguageUpdated", (sender, arg) =>
            {
                var split = arg.Split(',');
                if (split.Count() > 0)
                {
                    viewmodel.IsLoading = true;
                    viewmodel.Language = split[0];
                    CultureInfo language = new CultureInfo(split[0]);
                    Thread.CurrentThread.CurrentUICulture = language;
                    AppResources.Culture = language;
                    Device.BeginInvokeOnMainThread(() => 
                    { Xamarin.Forms.Application.Current.MainPage = new Xamarin.Forms.NavigationPage(new DrawerMaster("setting")); });
                }
            });
            #endregion
        }

        public void SetupView()
        {
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Xamarin.Forms.Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) frame_profile.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) frame_profile.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) frame_profile.BackgroundColor = Color.FromHex(App.DimGray2);
            }
            else frame_profile.BackgroundColor = Color.FromHex(App.DimGray2);

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
            lbl_version.Text = AppResources.setting_version + " " + App.AppVersion;
        }

        public async void DrawerTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var drawer = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
            drawer.IsPresented = true;
            view.IsEnabled = true;
        }

        public async void TopCommitmentBadgeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            var drawer = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
            drawer.IsPresented = true;
            view.IsEnabled = true;
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
            if (result == 5) SetDisplayAlert(AppResources.app_permission, AppResources.camgal_permmedia, "", "", "");
            if (result == 4) SetDisplayAlert(AppResources.app_permission, AppResources.camgal_permphoto, "", "", "");
            if (result == 3) SetDisplayAlert(AppResources.app_unavailable, AppResources.camgal_photounavailable, "", "", "");
            else if (result == 2)
            {
                img_profile.Source = "";
                SetDisplayAlert(AppResources.app_alert, AppResources.camgal_nophotoselect, "", "", "");
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
            if (result == 5) SetDisplayAlert(AppResources.app_permission, AppResources.camgal_permmedia, "", "", "");
            if (result == 4) SetDisplayAlert(AppResources.app_permission, AppResources.camgal_permphoto, "", "", "");
            if (result == 3) SetDisplayAlert(AppResources.app_unavailable, AppResources.camgal_camunavailable, "", "", "");
            else if (result == 2)
            {
                img_profile.Source = "";
                SetDisplayAlert(AppResources.app_alert, AppResources.camgal_camcancel, "", "", "");
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

        public async void CurrencyClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushPopupAsync(new CurrencyList());
            view.IsEnabled = true;
        }

        public async void LanguageClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushPopupAsync(new LanguageList());
            view.IsEnabled = true;
        }

        public async void AppThemeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var stack = (StackLayout)view.Content;
            var lbltheme = (Label)stack.Children[2];

            if (lbltheme.Text.Equals("1")) { viewmodel.Theme = "dark"; }
            else if (lbltheme.Text.Equals("2")) { viewmodel.Theme = "dim"; }
            else if (lbltheme.Text.Equals("3")) { viewmodel.Theme = "light"; }

            viewmodel.IsLoading = true;
            var result = await viewmodel.UpdateAppTheme();
            if (result == 1)
            {
                viewmodel.IsLoading = false;
                SetupView();
            }
            if (result == 2) SetDisplayAlert(AppResources.app_error, AppResources.code_setting_themefailed, "", "", "");
            view.IsEnabled = true;
        }

        public async void SaveClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            //if (viewmodel.Picture == null) SetDisplayAlert(AppResources.app_alert, "Profile picture is empty. Please take a photo or choose a picture.", "", "", "");
            if (string.IsNullOrEmpty(viewmodel.Firstname)) SetDisplayAlert(AppResources.app_alert, AppResources.code_setting_fnameempty, "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Lastname)) SetDisplayAlert(AppResources.app_alert, AppResources.code_setting_lnameempty, "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Username)) SetDisplayAlert(AppResources.app_alert, AppResources.code_setting_usernameempty, "", "", "");
            else if (viewmodel.Username.Length < 6) SetDisplayAlert(AppResources.app_alert, AppResources.code_setting_usermore6, "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Password)) SetDisplayAlert(AppResources.app_alert, AppResources.code_setting_pwdempty, "", "", "");
            else if (viewmodel.Password.Length < 6) SetDisplayAlert(AppResources.app_alert, AppResources.code_setting_pwdmore6, "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Repassword)) SetDisplayAlert(AppResources.app_confirmpwd, AppResources.code_setting_retypepwd, "", "", "");
            else if (!viewmodel.Repassword.Equals(viewmodel.Password))
            {
                SetDisplayAlert(AppResources.app_notmatch, AppResources.code_setting_pwdnotmatch, "", "", "");
                viewmodel.Repassword = string.Empty;
            }
            else
            {
                var result = viewmodel.UpdateSetting();
                if (result == 1)
                {
                    SetDisplayAlert(AppResources.app_success, AppResources.code_setting_profupdated, "", "", "");
                    viewmodel.IsViewing = true;
                    viewmodel.IsEditing = false;
                }
                else if (result == 2) SetDisplayAlert(AppResources.app_error, AppResources.code_setting_lognotfound, "", "", "");
                else if (result == 3) SetDisplayAlert(AppResources.app_error, AppResources.code_setting_updatefailed, "", "", "");
                else if (result == 4) SetDisplayAlert(AppResources.app_exist, AppResources.code_setting_edyexist, "", "", "");
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