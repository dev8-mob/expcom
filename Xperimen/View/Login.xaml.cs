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
        public Login()
        {
            InitializeComponent();
            BindingContext = new LoginViewmodel();
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
                await DisplayAlert("hanat", "masuk usernama la hanjink", "OK");
                entry_username.Isfocus = true;
            }
            else if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("hanat", "masuk password la lanjiao", "OK");
                entry_password.Isfocus = true;
            }
            else await Navigation.PushAsync(new MainPage());
        }
    }
}