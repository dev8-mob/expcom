using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
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

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) => { viewmodel.IsLoading = false; });
            MessagingCenter.Subscribe<SettingInfo>(this, "SettingEditProfile", (sender) => { SetupView(); });
        }

        public void SetupView()
        {
            img_profile.Source = ImageSource.FromStream(() =>
            {
                var stream = converter.BytesToStream(viewmodel.Picture);
                return stream;
            });

            if (viewmodel.Theme.Equals("dark"))
            {
                frame_profile.BackgroundColor = Color.Transparent;
                frame_dark.BackgroundColor = Color.FromHex(App.SlateGray);
                frame_dim.BackgroundColor = Color.Transparent;
                frame_light.BackgroundColor = Color.Transparent;
                frame_dark.BorderColor = Color.FromHex(App.Primary);
                frame_dim.BorderColor = Color.DarkGray;
                frame_light.BorderColor = Color.DarkGray;
            }
            else if (viewmodel.Theme.Equals("dim"))
            {
                frame_profile.BackgroundColor = Color.Transparent;
                frame_dark.BackgroundColor = Color.Transparent;
                frame_dim.BackgroundColor = Color.White;
                frame_light.BackgroundColor = Color.Transparent;
                frame_dark.BorderColor = Color.DarkGray;
                frame_dim.BorderColor = Color.FromHex(App.Primary);
                frame_light.BorderColor = Color.DarkGray;
            }
            else if (viewmodel.Theme.Equals("light"))
            {
                frame_profile.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_dark.BackgroundColor = Color.Transparent;
                frame_dim.BackgroundColor = Color.Transparent;
                frame_light.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_dark.BorderColor = Color.DarkGray;
                frame_dim.BorderColor = Color.DarkGray;
                frame_light.BorderColor = Color.FromHex(App.Primary);
            }
        }

        public async void ProfilePicClicked(object sender, EventArgs e)
        {
            if (viewmodel.Picture != null)
            {
                var view = (Frame)sender;
                await view.ScaleTo(0.9, 100);
                view.Scale = 1;
                view.IsEnabled = false;
                await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(viewmodel.Picture)));
                view.IsEnabled = true;
            }
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

        public async void ThemeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            var lbl = (Label)view.Content;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            var apptheme = "light";
            if (Application.Current.Properties.ContainsKey("app_theme"))
                apptheme = Application.Current.Properties["app_theme"] as string;

            #region update UI
            if (lbl.Text.Equals("Dark Theme"))
            {
                viewmodel.Theme = "dark";
                if (apptheme.Equals("dark")) frame_dark.BackgroundColor = Color.FromHex(App.SlateGray);
                else if (apptheme.Equals("dim")) frame_dark.BackgroundColor = Color.White;
                else if (apptheme.Equals("light")) frame_dark.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_dim.BackgroundColor = Color.Transparent;
                frame_light.BackgroundColor = Color.Transparent;
                frame_dark.BorderColor = Color.FromHex(App.Primary);
                frame_dim.BorderColor = Color.DarkGray;
                frame_light.BorderColor = Color.DarkGray;
            }
            else if (lbl.Text.Equals("Dim Theme"))
            {
                viewmodel.Theme = "dim";
                frame_dark.BackgroundColor = Color.Transparent;
                if (apptheme.Equals("dark")) frame_dim.BackgroundColor = Color.FromHex(App.SlateGray);
                else if (apptheme.Equals("dim")) frame_dim.BackgroundColor = Color.White;
                else if (apptheme.Equals("light")) frame_dim.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_light.BackgroundColor = Color.Transparent;
                frame_dark.BorderColor = Color.DarkGray;
                frame_dim.BorderColor = Color.FromHex(App.Primary);
                frame_light.BorderColor = Color.DarkGray;
            }
            else if (lbl.Text.Equals("Light Theme"))
            {
                viewmodel.Theme = "light";
                frame_dark.BackgroundColor = Color.Transparent;
                frame_dim.BackgroundColor = Color.Transparent;
                if (apptheme.Equals("dark")) frame_light.BackgroundColor = Color.FromHex(App.SlateGray);
                else if (apptheme.Equals("dim")) frame_light.BackgroundColor = Color.White;
                else if (apptheme.Equals("light")) frame_light.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_dark.BorderColor = Color.DarkGray;
                frame_dim.BorderColor = Color.DarkGray;
                frame_light.BorderColor = Color.FromHex(App.Primary);
            }
            #endregion

            viewmodel.IsLoading = true;
            var result = await viewmodel.UpdateAppTheme();
            if (result == 1) viewmodel.IsLoading = false;
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
            if (viewmodel.Picture == null) SetDisplayAlert("Alert", "Profile picture is empty. Please take a photo or choose a picture.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Firstname)) SetDisplayAlert("Alert", "First name cannot be empty. Please insert your first name.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Lastname)) SetDisplayAlert("Alert", "Last name cannot be empty. Please insert your last name.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Username)) SetDisplayAlert("Alert", "Username cannot be empty. Please insert your username.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Password)) SetDisplayAlert("Alert", "Password cannot be empty. Please insert your password.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Repassword)) SetDisplayAlert("Confirmation Password", "Please re-type your password.", "", "", "");
            else if (!viewmodel.Repassword.Equals(viewmodel.Password))
            {
                SetDisplayAlert("Not Match", "Confirmation password is not match with current password.", "", "", "");
                viewmodel.Repassword = string.Empty;
            }
            else if (string.IsNullOrEmpty(viewmodel.Description)) 
                SetDisplayAlert("Alert", "Description cannot be empty. Please provide any description about you.", "", "", "");
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
            viewmodel.SetupData();
            viewmodel.IsViewing = true;
            viewmodel.IsEditing = false;
            view.IsEnabled = true;
        }

        public async void BackTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var drawer = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
            drawer.IsPresented = true;
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