using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xperimen.Helper;
using Xperimen.Model;

namespace Xperimen.ViewModel
{
    public class CreateaccViewmodel : BaseViewModel
    {
        #region properties
        string _username;
        string _firstname;
        string _lastname;
        string _password;
        string _description;
        string _theme;
        MediaFile _picture;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
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
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        public string Theme
        {
            get { return _theme; }
            set { _theme = value; OnPropertyChanged(); }
        }
        public MediaFile Picture
        {
            get { return _picture; }
            set { _picture = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;

        public CreateaccViewmodel()
        {
            Firstname = string.Empty;
            Lastname = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            Description = string.Empty;
            Picture = null;
            connection = new SQLiteConnection(App.DB_PATH);
        }

        public async Task<int> TakePhoto()
        {
            Picture = null;
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) return 3;
            Picture = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Xperimen",
                PhotoSize = PhotoSize.Large, ////Resize to 75% of original
                CompressionQuality = 100,
                Name = Guid.NewGuid().ToString().Substring(0, 10) + ".jpg",
                SaveToAlbum = true
                //CustomPhotoSize = 90, //Resize to 90% of original
            });

            if (Picture == null) return 2;
            else return 1;
        }

        public async Task<int> PickPhoto()
        {
            Picture = null;
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported || !CrossMedia.Current.IsPickVideoSupported) return 3;
            Picture = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Large, ////Resize to 75% of original
                CompressionQuality = 100,
            });

            if (Picture == null) return 2;
            else return 1;
        }

        public async Task<int> CreateAccount()
        {
            string query = "SELECT * FROM Clients WHERE Username = '" + Username + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0) return 1;
            else
            {
                var convert = new StreamByteConverter();
                var data = new Clients
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = Username,
                    Firstname = string.Empty,
                    Lastname = string.Empty,
                    Password = Password,
                    Description = Description,
                    AppTheme = Theme,
                    ProfileImage = convert.GetImageBytes(Picture.GetStream()),
                    IsLogin = false
                };
                var camelcase = new CamelCaseChecker();
                data.Firstname = camelcase.CapitalizeWord(Firstname);
                data.Lastname = camelcase.CapitalizeWord(Lastname);
                connection.Insert(data);
                Application.Current.Properties["app_theme"] = Theme;
                await Application.Current.SavePropertiesAsync();

                try { MessagingCenter.Send(this, "AppThemeUpdated"); }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    var stack = ex.StackTrace;
                    var page = Application.Current.MainPage;
                    await page.DisplayAlert(error, stack, "OK");
                }

                Picture = null;
                Username = string.Empty;
                Firstname = string.Empty;
                Lastname = string.Empty;
                Password = string.Empty;
                Description = string.Empty;
                Theme = string.Empty;
                return 2;
            }
        }
    }
}
