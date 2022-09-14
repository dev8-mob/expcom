using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.View.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSetting : ContentPage
    {
        public MainSetting()
        {
            InitializeComponent();
        }

        private async void LightThemeTapped(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;
            MessagingCenter.Send(this, "SelectedTheme", "light");
        }

        private async void DarkThemeTapped(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;
            MessagingCenter.Send(this, "SelectedTheme", "dark");
        }
    }
}