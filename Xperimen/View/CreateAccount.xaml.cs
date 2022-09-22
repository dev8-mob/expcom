
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Stylekit;
using Xperimen.ViewModel;
using System;

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

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            { 
                viewmodel.IsLoading = false;
                if (arg.Equals("Okay")) Navigation.PopAsync();
            });
        }

        public async void ThemeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            var lbl = (Label)view.Content;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;

            var apptheme = "light";
            if (Application.Current.Properties.ContainsKey("app_theme"))
                apptheme = Application.Current.Properties["app_theme"] as string;

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
        }

        public async void CreateAccClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;

            viewmodel.IsLoading = true;
            if (string.IsNullOrEmpty(viewmodel.Username)) SetDisplayAlert("Alert", "Username cannot be empty. Please choose a username.", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Password)) SetDisplayAlert("Alert", "Password cannot be empty. Please insert your password.", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Description)) SetDisplayAlert("Alert", "Please provide any description about you.", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Theme)) SetDisplayAlert("Alert", "Please choose application theme.", "", "");
            else
            {
                var result = await viewmodel.CreateAccount();
                if (result == 1) SetDisplayAlert("Alert", "The username is already exist. Please choose different username.", "", "");
                else if (result == 2)
                {
                    MessagingCenter.Send(this, "AppThemeUpdated");
                    SetDisplayAlert("Success", "Successfully created your account.", "", "Okay");
                    frame_dark.BackgroundColor = Color.Transparent;
                    frame_dim.BackgroundColor = Color.Transparent;
                    frame_light.BackgroundColor = Color.Transparent;
                    frame_dark.BorderColor = Color.DarkGray;
                    frame_dim.BorderColor = Color.DarkGray;
                    frame_light.BorderColor = Color.DarkGray;
                    lbl_cancel.Text = "Go To Login";
                }
            }
        }

        public async void CancelClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;
            await Navigation.PopAsync();
        }

        public void SetDisplayAlert(string title, string description, string btn1, string btn2)
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
        }
    }
}