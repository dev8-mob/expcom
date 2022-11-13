
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Resources;

namespace Xperimen.View.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserlistCell : StackLayout
    {
        #region bindables
        public bool Login
        {
            get => (bool)GetValue(LoginProperty);
            set { SetValue(LoginProperty, value); }
        }
        public bool OnetimeLogin
        {
            get => (bool)GetValue(OnetimeLoginProperty);
            set { SetValue(OnetimeLoginProperty, value); }
        }
        public byte[] Picture
        {
            get => (byte[])GetValue(PictureProperty);
            set { SetValue(PictureProperty, value); }
        }
        public static BindableProperty LoginProperty =
            BindableProperty.Create(nameof(Login), typeof(bool), typeof(UserlistCell), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((UserlistCell)bindable).UpdateIsLogin((bool)newVal); });
        public static BindableProperty OnetimeLoginProperty =
            BindableProperty.Create(nameof(OnetimeLogin), typeof(bool), typeof(UserlistCell), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((UserlistCell)bindable).UpdateOnetimeLogin((bool)newVal); });
        public static BindableProperty PictureProperty =
            BindableProperty.Create(nameof(Picture), typeof(byte[]), typeof(UserlistCell), defaultValue: null,
                propertyChanged: (bindable, oldVal, newVal) => { ((UserlistCell)bindable).UpdatePicture((byte[])newVal); });
        public void UpdateIsLogin(bool data)
        {
            lbl_login.IsVisible = data;
            img_delete.IsVisible = !data;
        }
        public void UpdateOnetimeLogin(bool data)
        {
            if (OnetimeLogin && !Login) stack_lastlogin.IsVisible = true;
            else stack_lastlogin.IsVisible = false;
        }
        public void UpdatePicture(byte[] data)
        {
            if (data != null)
            {
                img_profile.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(data);
                    return stream;
                });
            }
        }
        #endregion

        public SQLiteConnection connection;
        public StreamByteConverter converter;

        public UserlistCell()
        {
            InitializeComponent();
            connection = new SQLiteConnection(App.DB_PATH);
            converter = new StreamByteConverter();
        }

        public async void HeaderTapped(object sender, EventArgs arg)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            if (stack_detail.IsVisible) stack_detail.IsVisible = false;
            else if (!stack_detail.IsVisible) stack_detail.IsVisible = true;
        }

        public async void DeleteTapped(object sender, EventArgs arg)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var parent = (AccountList)view.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent;

            var fullname = lbl_fname.Text + " " + lbl_lname.Text;
            parent.viewmodel.IsLoading = true;
            parent.SetDisplayAlert(AppResources.app_delete, AppResources.code_setting_suredeluser + fullname + "?", AppResources.app_okay, AppResources.app_cancel, lbl_id.Text);
            // send the rest of the delete process to parent viewmodel
            view.IsEnabled = true;
        }

        public async void DeleteMeClicked(object sender, EventArgs arg)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var parent = (AccountList)view.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent;

            parent.viewmodel.IsLoading = true;
            parent.SetDisplayAlert(AppResources.app_delete, AppResources.code_setting_suredelself, AppResources.app_okay, AppResources.app_cancel, "deleteme");
            // send the rest of the delete process to parent viewmodel
            view.IsEnabled = true;
        }
    }
}