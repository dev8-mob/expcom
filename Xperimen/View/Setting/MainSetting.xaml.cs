using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel.Setting;

namespace Xperimen.View.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSetting : ContentPage
    {
        public SettingViewmodel viewmodel;

        public MainSetting()
        {
            InitializeComponent();
            viewmodel = new SettingViewmodel();
            BindingContext = viewmodel;
            SetupView();
        }

        public void SetupView()
        {
            if (viewmodel.Theme.Equals("dark"))
            {
                frame_dark.BackgroundColor = Color.FromHex(App.SlateGray);
                frame_dim.BackgroundColor = Color.Transparent;
                frame_light.BackgroundColor = Color.Transparent;
                frame_dark.BorderColor = Color.FromHex(App.Primary);
                frame_dim.BorderColor = Color.DarkGray;
                frame_light.BorderColor = Color.DarkGray;
            }
            else if (viewmodel.Theme.Equals("dim"))
            {
                frame_dark.BackgroundColor = Color.Transparent;
                frame_dim.BackgroundColor = Color.White;
                frame_light.BackgroundColor = Color.Transparent;
                frame_dark.BorderColor = Color.DarkGray;
                frame_dim.BorderColor = Color.FromHex(App.Primary);
                frame_light.BorderColor = Color.DarkGray;
            }
            else if (viewmodel.Theme.Equals("light"))
            {
                frame_dark.BackgroundColor = Color.Transparent;
                frame_dim.BackgroundColor = Color.Transparent;
                frame_light.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_dark.BorderColor = Color.DarkGray;
                frame_dim.BorderColor = Color.DarkGray;
                frame_light.BorderColor = Color.FromHex(App.Primary);
            }
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

        public async void BackTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;
            await Navigation.PopAsync();
        }
    }
}