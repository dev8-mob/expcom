
using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.View
{
    public class Logout : ContentPage
    {
        public Logout() { ClientLogout(); }

        public async void ClientLogout()
        {
            try
            {
                var connection = new SQLiteConnection(App.DB_PATH);
                var userid = Application.Current.Properties["current_login"] as string;
                var queryuser = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
                var user = connection.Query<Clients>(queryuser).ToList();

                if (user.Count > 0)
                {
                    var model = new Clients
                    {
                        Id = user[0].Id,
                        Username = user[0].Username,
                        Firstname = user[0].Firstname,
                        Lastname = user[0].Lastname,
                        Password = user[0].Password,
                        Description = user[0].Description,
                        ProfileImage = user[0].ProfileImage,
                        AppTheme = user[0].AppTheme,
                        AccountCreated = user[0].AccountCreated,
                        AccountUpdated = user[0].AccountUpdated,
                        Logout = user[0].Logout,
                        HaveUpdated = user[0].HaveUpdated,
                        IsLogin = false,
                        HaveOnetimeLogin = user[0].HaveOnetimeLogin,
                        Income = user[0].Income,
                        TotalCommitment = user[0].TotalCommitment,
                        NetIncome = user[0].NetIncome
                    };
                    model.Logout = DateTime.Now;
                    var result = connection.Update(model);
                    if (result == 1)
                    {
                        Application.Current.Properties.Remove("current_login");
                        await Application.Current.SavePropertiesAsync();
                        Application.Current.MainPage = new NavigationPage(new Login());
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
            }
        }
    }
}
