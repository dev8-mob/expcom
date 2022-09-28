﻿using System;
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

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            { viewmodel.IsLoading = false; });
        }

        public async void LoginClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            viewmodel.IsLoading = true;
            if (string.IsNullOrEmpty(viewmodel.Username)) SetDisplayAlert("Alert", "Please insert your username.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Password)) SetDisplayAlert("Alert", "Please insert your password.", "", "", "");
            else
            {
                var result = await viewmodel.Login();
                if (result == 1) Application.Current.MainPage = new NavigationPage(new DrawerMaster());
                else if (result == 2) SetDisplayAlert("Alert", "Your password is incorrect. Please insert the correct password.", "", "", "");
                else if (result == 3) SetDisplayAlert("Alert", "The username is not found.", "", "", "");
            }
        }

        public async void CreateAccClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            await Navigation.PushAsync(new CreateAccount());
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