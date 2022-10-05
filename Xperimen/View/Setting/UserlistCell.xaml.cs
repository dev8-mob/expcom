
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;

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
        public byte[] Picture
        {
            get => (byte[])GetValue(PictureProperty);
            set { SetValue(PictureProperty, value); }
        }
        public static BindableProperty PictureProperty =
            BindableProperty.Create(nameof(Picture), typeof(byte[]), typeof(UserlistCell), defaultValue: null,
                propertyChanged: (bindable, oldVal, newVal) => { ((UserlistCell)bindable).UpdatePicture((byte[])newVal); });
        public static BindableProperty LoginProperty =
            BindableProperty.Create(nameof(Login), typeof(bool), typeof(UserlistCell), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((UserlistCell)bindable).UpdateIsLogin((bool)newVal); });
        public void UpdatePicture(byte[] data)
        {
            img_profile.Source = ImageSource.FromStream(() =>
            {
                var stream = converter.BytesToStream(data);
                return stream;
            });
        }
        public void UpdateIsLogin(bool data)
        {
            lbl_login.IsVisible = data;
            img_delete.IsVisible = !data;
            stack_lastlogin.IsVisible = !data;
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
            var parent = (AccountList)view.Parent.Parent.Parent.Parent.Parent.Parent;

            var fullname = lbl_fname.Text + " " + lbl_lname.Text;
            parent.viewmodel.IsLoading = true;
            parent.SetDisplayAlert("Delete", "Are you sure to delete " + fullname + "?", "Okay", "Cancel", lbl_id.Text);
            // send the rest of the delete process to parent viewmodel
            view.IsEnabled = true;
        }
    }
}