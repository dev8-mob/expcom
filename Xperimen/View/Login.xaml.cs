using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Stylekit;
using Xperimen.View.NavigationDrawer;
using Xperimen.ViewModel;
using Xperimen.Model;
using SQLite;
using System.Linq;

namespace Xperimen.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public LoginViewmodel viewmodel;
        public SQLiteConnection connection;

        public Login()
        {
            InitializeComponent();
            viewmodel = new LoginViewmodel();
            connection = new SQLiteConnection(App.DB_PATH);
            BindingContext = viewmodel;

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            { viewmodel.IsLoading = false; });
        }

        public async void SubmitClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;

            var user = entry_username.GetText();
            var password = entry_password.GetText();

            viewmodel.IsLoading = true;
            if (string.IsNullOrEmpty(user)) SetDisplayAlert("Alert", "Please insert your username.", "", "");
            else if (string.IsNullOrEmpty(password)) SetDisplayAlert("Alert", "Please insert your password.", "", "");
            else
            {
                string query = "SELECT * FROM Clients WHERE Username = '" + user + "' AND Password = '" + password + "'";
                var result = connection.Query<Clients>(query).ToList();
                if (result.Count > 0)
                {
                    var login = new ClientCurrent
                    {
                        UserId = result[0].Id,
                        Username = result[0].Username,
                        Description = result[0].Description
                    };
                    connection.DeleteAll<ClientCurrent>();
                    connection.Insert(login);
                    Application.Current.MainPage = new NavigationPage(new DrawerMaster());
                }
                else
                {
                    query = "SELECT * FROM Clients WHERE Username = '" + user + "'";
                    result = connection.Query<Clients>(query).ToList();
                    if (result.Count > 0)
                    {
                        SetDisplayAlert("Alert", "Your password is incorrect. Please insert the correct password.", "", "");
                        entry_password.Text = string.Empty;
                    }
                    else SetDisplayAlert("Alert", "The username is not found.", "", "");
                }
            }
        }

        public async void CreateAccClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;
            await Navigation.PushAsync(new CreateAccount());
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