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
            await Navigation.PushAsync(new MainPage());
        }
    }
}