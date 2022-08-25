using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Stylekit;
using Xperimen.View.Dashboard;
using Xperimen.ViewModel;

namespace Xperimen.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public LoginViewmodel model;

        public Login()
        {
            InitializeComponent();
            model = new LoginViewmodel();
            BindingContext = model;

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            {
                model.IsLoading = false;
            });
        }

        public async void OnTestTapped(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;
            model.IsLoading = false;
        }

        public async void Submit_Clicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;

            var user = entry_username.GetText();
            var password = entry_password.GetText();

            if (string.IsNullOrEmpty(user))
            {
                SetDisplayAlert("Username empty !?", "Where is your username !? Please insert your username.", "", "");
                entry_username.Isfocus = true;
            }
            else if (string.IsNullOrEmpty(password))
            {
                SetDisplayAlert("Password Empty !?", "Where is your password !? Please insert your password.", "", "Cancel");
                entry_password.Isfocus = true;
            }
            else await Navigation.PushAsync(new MainPage());
        }

        public void SetDisplayAlert(string title, string description, string btn1, string btn2)
        {
            model.IsLoading = true;
            alert.Title = title;
            alert.Description = description;
            alert.TxtBtn1 = btn1;
            alert.TxtBtn2 = btn2;
            alert.IsVisible = true;
        }
    }
}