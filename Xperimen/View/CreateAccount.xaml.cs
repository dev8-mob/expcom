
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Stylekit;
using Xperimen.ViewModel;

namespace Xperimen.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAccount : ContentPage
    {
        public CreateaccViewmodel viewmodel;

        public CreateAccount()
        {
            InitializeComponent();
            viewmodel = new CreateaccViewmodel();
            BindingContext = viewmodel;

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"];
                if (theme.Equals("dark")) frame_profile.BackgroundColor = Color.Transparent;
                if (theme.Equals("dim")) frame_profile.BackgroundColor = Color.Transparent;
                if (theme.Equals("light")) frame_profile.BackgroundColor = Color.FromHex(App.DimGray2);
            }

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            {
                viewmodel.IsLoading = false;
                if (arg.Equals("Okay")) Navigation.PopAsync();
            });
        }

        public async void ProfilePicClicked(object sender, EventArgs e)
        {
            if (viewmodel.Picture != null)
            {
                var view = (Frame)sender;
                await view.ScaleTo(0.9, 100);
                view.Scale = 1;
                view.IsEnabled = false;
                await Navigation.PushPopupAsync(new ImageViewer(viewmodel.Picture.GetStream()));
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
            else if (result == 2) SetDisplayAlert("Alert", "No photo selected.", "", "", "");
            else if (result == 1)
            {
                img_profile.Source = ImageSource.FromStream(() =>
                {
                    var stream = viewmodel.Picture.GetStream();
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
            else if (result == 2) SetDisplayAlert("Alert", "Take photo cancelled.", "", "", "");
            else if (result == 1)
            {
                img_profile.Source = ImageSource.FromStream(() =>
                {
                    var stream = viewmodel.Picture.GetStream();
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

            if (lbltheme.Text.Equals("Dark")) viewmodel.Theme = "dark";
            else if (lbltheme.Text.Equals("Dim")) viewmodel.Theme = "dim";
            else if (lbltheme.Text.Equals("Light")) viewmodel.Theme = "light";
            view.IsEnabled = true;
        }

        public async void CreateAccClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            if (viewmodel.Picture == null) SetDisplayAlert("Alert", "Profile picture is empty. Please take a photo or choose a picture.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Firstname)) SetDisplayAlert("Alert", "First name is empty. Please insert your first name.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Lastname)) SetDisplayAlert("Alert", "Last name is empty. Please insert your last name.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Username)) SetDisplayAlert("Alert", "Username cannot be empty. Please choose a username.", "", "", "");
            else if (viewmodel.Username.Length < 6) SetDisplayAlert("Alert", "Username must be more than 6 characters.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Password)) SetDisplayAlert("Alert", "Password cannot be empty. Please insert your password.", "", "", "");
            else if (viewmodel.Password.Length < 6) SetDisplayAlert("Alert", "Password must be more than 6 characters.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Description)) SetDisplayAlert("Alert", "Please provide any description about you.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Theme)) SetDisplayAlert("Alert", "Please choose application theme.", "", "", "");
            else
            {
                try
                {
                    var result = await viewmodel.CreateAccount();
                    if (result == 1) SetDisplayAlert("Alert", "The username is already exist. Please choose different username.", "", "", "");
                    else if (result == 2)
                    {
                        SetDisplayAlert("Success", "Successfully created your account.", "", "Okay", "");
                        lbl_cancel.Text = "Go To Login";
                        img_profile.Source = "";
                    }
                }
                catch (Exception ex)
                {
                    viewmodel.IsLoading = false;
                    //await Navigation.PopAsync();
                    var error = ex.Message;
                    var desc = ex.StackTrace;
                    //await DisplayAlert(error, desc, "OK!");
                }
            }
            view.IsEnabled = true;
        }

        public async void CancelClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PopAsync();
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