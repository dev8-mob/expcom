
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Stylekit;
using Xperimen.ViewModel.Setting;

namespace Xperimen.View.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingInfo : Frame
    {
        public SettingViewmodel viewmodel;

        public SettingInfo()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) frame_profile.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) frame_profile.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) frame_profile.BackgroundColor = Color.FromHex(App.DimGray2);
            }
            else frame_profile.BackgroundColor = Color.FromHex(App.DimGray2);

            var convert = new StreamByteConverter();
            if (viewmodel.Picture != null)
            {
                img_profile.Source = ImageSource.FromStream(() =>
                {
                    var stream = convert.BytesToStream(viewmodel.Picture);
                    return stream;
                });
            }
        }

        public void SetParentBinding(SettingViewmodel parent)
        {
            viewmodel = parent;
            SetupView();
        }

        public async void ProfilePicClicked(object sender, EventArgs e)
        {
            await frame_profile.ScaleTo(0.9, 100);
            frame_profile.Scale = 1;
            frame_profile.IsEnabled = false;
            if (viewmodel.Picture != null)
            {
                var convert = new StreamByteConverter();
                await Navigation.PushPopupAsync(new ImageViewer(convert.BytesToStream(viewmodel.Picture)));
            }
            frame_profile.IsEnabled = true;
        }

        public async void EditClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            viewmodel.IsViewing = false;
            viewmodel.IsEditing = true;
            MessagingCenter.Send(this, "SettingEditProfile");
            view.IsEnabled = true;
        }
    }
}