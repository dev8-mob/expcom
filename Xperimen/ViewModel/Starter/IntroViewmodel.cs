using SQLite;
using System.Linq;
using System;
using Xperimen.Helper;
using Xperimen.Model;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;
using System.Threading;
using Xperimen.Resources;

namespace Xperimen.ViewModel.Starter
{
    public class IntroViewmodel : BaseViewModel
    {
        #region properties
        string _firstname;
        string _lastname;
        string _username;
        string _password;
        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; OnPropertyChanged(); }
        }
        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; OnPropertyChanged(); }
        }
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

        public IntroViewmodel()
        {
            Firstname = string.Empty;
            Lastname = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            connection = new SQLiteConnection(App.DB_PATH);
        }

        public async Task<int> IntroProfile()
        {
            try
            {
                string query = "SELECT * FROM Clients WHERE Username = '" + Username + "'";
                var result = connection.Query<Clients>(query).ToList();
                if (result.Count > 0) return 2;
                else
                {
                    var convert = new StreamByteConverter();
                    var data = new Clients
                    {
                        Id = Guid.NewGuid().ToString(),
                        Username = string.Empty,
                        Firstname = string.Empty,
                        Lastname = string.Empty,
                        Password = Password,
                        Description = string.Empty,
                        ProfileImage = null,
                        AppTheme = "light",
                        AccountCreated = DateTime.Now,
                        AccountUpdated = DateTime.Now,
                        Logout = new DateTime(1, 1, 1),
                        IsLogin = true,
                        HaveOnetimeLogin = true,
                        HaveUpdated = false,
                        Income = 0,
                        TotalCommitment = 0,
                        NetIncome = 0,
                        Currency = "RM",
                        Language = "en"
                    };
                    var camelcase = new CamelCaseChecker();
                    Username = Firstname.Trim().ToLower() + Lastname.Trim().ToLower();
                    data.Username = Username;
                    data.Firstname = camelcase.CapitalizeWord(Firstname);
                    data.Lastname = camelcase.CapitalizeWord(Lastname);
                    
                    connection.Insert(data);
                    CultureInfo language = new CultureInfo("en");
                    Thread.CurrentThread.CurrentUICulture = language;
                    AppResources.Culture = language;
                    Application.Current.Properties["current_login"] = data.Id;
                    Application.Current.Properties["app_theme"] = data.AppTheme;
                    Application.Current.Properties["firstmonth_isreset"] = "false";
                    await Application.Current.SavePropertiesAsync();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 3;
            }
        }

        public async Task<int> SkipIntroProfile()
        {
            try
            {
                var guid = Guid.NewGuid().ToString();
                Firstname = guid.Substring(0, 3);
                Lastname = guid.Substring(3, 3);
                Password = guid.Substring(0, 6);
                var convert = new StreamByteConverter();
                var data = new Clients
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = string.Empty,
                    Firstname = string.Empty,
                    Lastname = string.Empty,
                    Password = Password,
                    Description = string.Empty,
                    ProfileImage = null,
                    AppTheme = "light",
                    AccountCreated = DateTime.Now,
                    AccountUpdated = DateTime.Now,
                    Logout = new DateTime(1, 1, 1),
                    IsLogin = true,
                    HaveOnetimeLogin = true,
                    HaveUpdated = false,
                    Income = 0,
                    TotalCommitment = 0,
                    NetIncome = 0,
                    Currency = "RM",
                    Language = "en"
                };
                var camelcase = new CamelCaseChecker();
                Username = Firstname.Trim().ToLower() + Lastname.Trim().ToLower();
                data.Username = Username;
                data.Firstname = camelcase.CapitalizeWord(Firstname);
                data.Lastname = camelcase.CapitalizeWord(Lastname);

                connection.Insert(data);
                CultureInfo language = new CultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = language;
                AppResources.Culture = language;
                Application.Current.Properties["current_login"] = data.Id;
                Application.Current.Properties["app_theme"] = data.AppTheme;
                Application.Current.Properties["firstmonth_isreset"] = "false";
                await Application.Current.SavePropertiesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }
    }
}
