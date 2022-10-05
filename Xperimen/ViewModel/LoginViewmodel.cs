
using SQLite;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel
{
    public class LoginViewmodel : BaseViewModel
    {
        #region properties
        string _username;
        string _password;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;

        public LoginViewmodel()
        {
            Username = string.Empty;
            Password = string.Empty;
            connection = new SQLiteConnection(App.DB_PATH);
        }

        public async Task<int> Login()
        {
            string query = "SELECT * FROM Clients WHERE Username = '" + Username + "' AND Password = '" + Password + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0)
            {
                var model = new Clients
                {
                    Id = result[0].Id,
                    Username = result[0].Username,
                    Firstname = result[0].Firstname,
                    Lastname = result[0].Lastname,
                    Password = result[0].Password,
                    Description = result[0].Description,
                    ProfileImage = result[0].ProfileImage,
                    AppTheme = result[0].AppTheme,
                    AccountCreated = result[0].AccountCreated,
                    AccountUpdated = result[0].AccountUpdated,
                    Logout = DateTime.Now,
                    IsLogin = true,
                    Income = result[0].Income,
                    TotalCommitment = result[0].TotalCommitment,
                    NetIncome = result[0].NetIncome
                };
                var success = connection.Update(model);
                if (success == 1)
                {
                    var cek = connection.Table<Clients>().ToList();
                    Application.Current.Properties["current_login"] = result[0].Id;
                    Application.Current.Properties["app_theme"] = result[0].AppTheme;
                    await Application.Current.SavePropertiesAsync();

                    try { MessagingCenter.Send(this, "AppThemeUpdated"); }
                    catch (Exception ex)
                    {
                        var error = ex.Message;
                        var stack = ex.StackTrace;
                        var page = Application.Current.MainPage;
                        await page.DisplayAlert(error, stack, "OK");
                    }
                    return 1;
                }
                else return 4;
            }
            else
            {
                query = "SELECT * FROM Clients WHERE Username = '" + Username + "'";
                result = connection.Query<Clients>(query).ToList();
                if (result.Count > 0)
                {
                    Password = string.Empty;
                    return 2;
                }
                else return 3;
            }
        }
    }
}
