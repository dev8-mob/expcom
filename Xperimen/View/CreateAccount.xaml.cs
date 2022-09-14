
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.Stylekit;
using Xperimen.ViewModel;
using SQLite;
using System;
using System.Linq;

namespace Xperimen.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAccount : ContentPage
    {
        public CreateaccViewmodel viewmodel;
        public SQLiteConnection connection;
        public string theme;

        public CreateAccount()
        {
            InitializeComponent();
            viewmodel = new CreateaccViewmodel();
            connection = new SQLiteConnection(App.DB_PATH);
            BindingContext = viewmodel;

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            { viewmodel.IsLoading = false; });
        }

        public async void ThemeClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;

            if (view.Text.Equals("Dark Theme")) theme = "dark";
            else if (view.Text.Equals("Light Theme")) theme = "light";
        }

        public async void CreateAccClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;

            var username = entry_username.GetText();
            var password = entry_password.GetText();
            var desc = editor_desc.GetText();

            if (string.IsNullOrEmpty(username))
            {
                viewmodel.IsLoading = true;
                SetDisplayAlert("Alert", "Username cannot be empty. Please choose a username.", "", "");
                entry_username.Isfocus = true;
            }
            else if (string.IsNullOrEmpty(password))
            {
                viewmodel.IsLoading = true;
                SetDisplayAlert("Alert", "Password cannot be empty. Please insert your password.", "", "");
                entry_password.Isfocus = true;
            }
            else if (string.IsNullOrEmpty(desc))
            {
                viewmodel.IsLoading = true;
                SetDisplayAlert("Alert", "Please provide any description about you.", "", "");
                editor_desc.Isfocus = true;
            }
            else
            {
                string query = "SELECT * FROM Clients WHERE Username = '" + username + "'";
                var result = connection.Query<Clients>(query).ToList();
                if (result.Count > 0)
                {
                    viewmodel.IsLoading = true;
                    SetDisplayAlert("Alert", "The username is already exist. Please choose different username.", "", "");
                }
                else
                {
                    var data = new Clients
                    {
                        Id = Guid.NewGuid().ToString(),
                        Username = username,
                        Password = password,
                        Description = desc,
                        AppTheme = theme
                    };
                    connection.Insert(data);
                    await DisplayAlert("Success", "Successfully created your account.", "OK");
                    await Navigation.PopAsync();
                }
            }
        }

        public async void CancelClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;
            await Navigation.PopAsync();
        }

        public void SetDisplayAlert(string title, string description, string btn1, string btn2)
        {
            //if string1 empty will not display btn1, if string2 empty will not display btn2
            //if both string1 & string2 empty will not display all buttons
            //all buttons tapped will send 'DisplayAlertSelection' with text of the button
            //close button tapped will send 'DisplayAlertSelection' with empty text
            alert.Title = title;
            alert.Description = description;
            alert.TxtBtn1 = btn1;
            alert.TxtBtn2 = btn2;
            alert.IsVisible = true;
        }
    }
}