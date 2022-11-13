using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Resources;
using Xperimen.Stylekit;
using Xperimen.View.NavigationDrawer;
using Xperimen.ViewModel.Starter;

namespace Xperimen.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Intro : ContentPage
    {
        public IntroViewmodel viewmodel;

        public Intro()
        {
            InitializeComponent();
            viewmodel = new IntroViewmodel();
            BindingContext = viewmodel;
            StartAnimation();
            lbl_version.Text = "Version " + App.AppVersion;

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", async (sender, arg) =>
            {
                if (arg.Equals("Okay"))
                {
                    if (alert.CodeObject.Equals("skip"))
                    {
                        var result = await viewmodel.SkipIntroProfile();
                        if (result == 1) Application.Current.MainPage = new NavigationPage(new DrawerMaster());
                        else if (result == 2) SetDisplayAlert("Error", "Technical error creating test account.", "", "", "");
                    }
                    else viewmodel.IsLoading = false;
                }
                else viewmodel.IsLoading = false;
            });
        }

        public async void StartAnimation()
        {
            await Task.Delay(300);
            await Task.Run(() =>
            {
                lbl_welcome.TranslateTo(0, 80, 1500, Easing.CubicOut);
                stack_welcome.TranslateTo(0, 80, 900, Easing.CubicOut);
            });
        }

        public async void NextClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            if (string.IsNullOrEmpty(viewmodel.Firstname)) SetDisplayAlert("Empty", "First name cannot be empty. Please insert first name.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Lastname)) SetDisplayAlert("Empty", "Last name cannot be empty. Please insert last name.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Password)) SetDisplayAlert("Empty", "Password cannot be empty. Please create your password.", "", "", "");
            else if (viewmodel.Password.Length < 6) SetDisplayAlert("Alert", "Password must be more than 6 characters.", "", "", "");
            else
            {
                var result = await viewmodel.IntroProfile();
                if (result == 1) Application.Current.MainPage = new NavigationPage(new DrawerMaster());
                else if (result == 2) SetDisplayAlert("Exist", "Account with the username already exist in this device.", "", "", "");
                else if (result == 3) SetDisplayAlert("Error", "Technical error creating new account.", "", "", "");
            }
            view.IsEnabled = true;
        }

        public async void SkipClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            viewmodel.IsLoading = true;
            SetDisplayAlert("Skip", "You can edit your info later in 'Settings' in the side menu.", "Okay", "", "skip");
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