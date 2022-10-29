using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Stylekit;
using Xperimen.View.NavigationDrawer;
using Xperimen.ViewModel;

namespace Xperimen.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public LoginViewmodel viewmodel;

        public Login()
        {
            InitializeComponent();
            viewmodel = new LoginViewmodel();
            BindingContext = viewmodel;
            SetupView();

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            { viewmodel.IsLoading = false; });
            MessagingCenter.Subscribe<CreateaccViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"];
                if (theme.Equals("dark")) frame_bg.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) frame_bg.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) frame_bg.BackgroundColor = Color.FromHex(App.DimGray2);
            }
            else frame_bg.BackgroundColor = Color.FromHex(App.DimGray2);
            lbl_version.Text = "Version " + App.AppVersion;
        }

        public async void LoginClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            if (string.IsNullOrEmpty(viewmodel.Username)) SetDisplayAlert("Alert", "Please insert your username.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Password)) SetDisplayAlert("Alert", "Please insert your password.", "", "", "");
            else
            {
                var result = await viewmodel.Login();
                if (result == 1) Application.Current.MainPage = new NavigationPage(new DrawerMaster());
                else if (result == 2) SetDisplayAlert("Alert", "Your password is incorrect. Please insert the correct password.", "", "", "");
                else if (result == 3) SetDisplayAlert("Alert", "The username is not found.", "", "", "");
                else if (result == 4) SetDisplayAlert("Error", "Technical error updating login information.", "", "", "");
            }
            view.IsEnabled = true;
        }

        public async void CreateAccClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushAsync(new CreateAccount());
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