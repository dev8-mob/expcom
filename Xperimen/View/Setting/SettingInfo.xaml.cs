
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
            viewmodel = new SettingViewmodel();
            BindingContext = viewmodel;
            SetupView();
        }

        public void SetupView()
        {
            var convert = new StreamByteConverter();
            img_profile.Source = ImageSource.FromStream(() =>
            {
                var stream = convert.BytesToStream(viewmodel.user_login.ProfileImage);
                return stream;
            });
            lbl_fullname.Text = viewmodel.Firstname + " " + viewmodel.Lastname;
            lbl_username.Text = "@" + viewmodel.Username;
        }

        public async void ProfilePicClicked(object sender, EventArgs e)
        {
            await frame_profile.ScaleTo(0.9, 100);
            frame_profile.Scale = 1;
            var convert = new StreamByteConverter();
            await Navigation.PushPopupAsync(new ImageViewer(convert.BytesToStream(viewmodel.user_login.ProfileImage)));
        }

        public async void EditClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            viewmodel.IsViewing = false;
            viewmodel.IsEditing = true;
        }
    }
}