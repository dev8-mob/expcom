using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
                model.IsLoading = true;
                SetDisplayAlert("Alert", "Please insert username", "Okay", "Not Okay");
                entry_username.Isfocus = true;
            }
            else if (string.IsNullOrEmpty(password))
            {
                model.IsLoading = true;
                SetDisplayAlert("Alert", "Please insert password", "", "Wartaparg");
                entry_password.Isfocus = true;
            }
            else await Navigation.PushAsync(new MainPage());
        }

        public void SetDisplayAlert(string title, string description, string btn1, string btn2)
        {
            alert.Title = title;
            alert.Description = description;
            alert.TxtBtn1 = btn1;
            alert.TxtBtn2 = btn2;
            alert.IsVisible = true;
        }
    }
}