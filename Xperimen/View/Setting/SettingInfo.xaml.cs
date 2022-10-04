
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
            var convert = new StreamByteConverter();
            img_profile.Source = ImageSource.FromStream(() =>
            {
                var stream = convert.BytesToStream(viewmodel.Picture);
                return stream;
            });
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
            var convert = new StreamByteConverter();
            await Navigation.PushPopupAsync(new ImageViewer(convert.BytesToStream(viewmodel.Picture)));
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