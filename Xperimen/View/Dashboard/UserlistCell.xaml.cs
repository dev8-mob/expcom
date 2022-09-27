
using System;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserlistCell : ViewCell
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
        }
        #endregion

        public SQLiteConnection connection;
        public StreamByteConverter converter;
        public UserList parent;

        public UserlistCell()
        {
            InitializeComponent();
            connection = new SQLiteConnection(App.DB_PATH);
            converter = new StreamByteConverter();

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) img_profile.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) img_profile.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) img_profile.BackgroundColor = Color.FromHex(App.DimGray2);
            }
            else img_profile.BackgroundColor = Color.FromHex(App.DimGray2);

            //MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            //{
            //    //'DisplayAlertSelection' comes from CustomDisplayAlert in AdminPage(parent) to delete a user
            //    if (arg.Equals("Okay")) 
            //    {
            //        if (lbl_login.IsVisible)
            //        {
            //            if (parent != null)
            //            {
            //                parent.viewmodel.IsLoading = true;
            //                //parent.SetDisplayAlert("Failed", "Cannot delete currently login user.", "", "");
            //            }
            //        }
            //        else
            //        {
            //            try
            //            {
            //                var query = "DELETE FROM Clients WHERE Id = '" + lbl_id.Text + "'";
            //                connection.Query<Clients>(query);
            //                var cek = connection.Table<Clients>().ToList();
            //                MessagingCenter.Send(this, "UpdateList");
            //            }
            //            catch (Exception ex)
            //            {
            //                var error = ex.Message;
            //                var desc = ex.StackTrace;
            //            }
            //        }
            //    }
            //});
        }

        public async void DeleteTapped(object sender, EventArgs arg)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            parent = (UserList)view.Parent.Parent.Parent.Parent.Parent.Parent.Parent;

            var fullname = lbl_fname.Text + " " + lbl_lname.Text;
            parent.viewmodel.IsLoading = true;
            parent.viewmodel.IsBlocked = true;
            parent.SetDisplayAlert("Delete", "Are you sure to delete " + fullname + "?", "Okay", "Cancel", lbl_id.Text);
            // rest of the delete process is in parent viewmodel
        }
    }
}