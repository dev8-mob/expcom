﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xperimen.Helper;
using Xperimen.Model;

namespace Xperimen.ViewModel.Setting
{
    public class SettingViewmodel : BaseViewModel
    {
        #region properties
        string _username;
        string _firstname;
        string _lastname;
        string _password;
        string _repassword;
        string _description;
        string _theme;
        byte[] _picture;
        bool _isediting;
        bool _isviewing;

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
        public string Repassword
        {
            get { return _repassword; }
            set { _repassword = value; OnPropertyChanged(); }
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
        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; OnPropertyChanged(); }
        }
        public bool IsEditing
        {
            get { return _isediting; }
            set { _isediting = value; OnPropertyChanged(); }
        }
        public bool IsViewing
        {
            get { return _isviewing; }
            set { _isviewing = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;
        public StreamByteConverter converter;

        public SettingViewmodel()
        {
            connection = new SQLiteConnection(App.DB_PATH);
            converter = new StreamByteConverter();
            IsViewing = true;
            IsEditing = false;
            SetupData();
        }

        public void SetupData()
        {
            var userid = Application.Current.Properties["current_login"] as string;
            string query = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0)
            {
                Username = result[0].Username;
                Firstname = result[0].Firstname;
                Lastname = result[0].Lastname;
                Password = result[0].Password;
                Description = result[0].Description;
                Picture = result[0].ProfileImage;
                Theme = result[0].AppTheme;
            }
        }

        public async Task<int> TakePhoto()
        {
            Picture = null;
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) return 3;

            var statusphoto = await Permissions.CheckStatusAsync<Permissions.Photos>();
            if (statusphoto != PermissionStatus.Granted) statusphoto = await Permissions.RequestAsync<Permissions.Photos>();
            if (statusphoto == PermissionStatus.Granted)
            {
                var statusmedia = await Permissions.CheckStatusAsync<Permissions.Media>();
                if (statusmedia != PermissionStatus.Granted) statusmedia = await Permissions.RequestAsync<Permissions.Media>();
                if (statusmedia != PermissionStatus.Granted) return 5;
                else
                {
                    var media = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Xperimen",
                        PhotoSize = PhotoSize.Large, ////Resize to 75% of original
                        CompressionQuality = 100,
                        Name = Guid.NewGuid().ToString().Substring(0, 10) + ".jpg",
                        SaveToAlbum = true
                        //CustomPhotoSize = 90, //Resize to 90% of original
                    });

                    if (media == null) return 2;
                    else
                    {
                        Picture = converter.GetImageBytes(media.GetStream());
                        return 1;
                    }
                }
            }
            else return 4;
        }

        public async Task<int> PickPhoto()
        {
            Picture = null;
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported || !CrossMedia.Current.IsPickVideoSupported) return 3;

            var statusphoto = await Permissions.CheckStatusAsync<Permissions.Photos>();
            if (statusphoto != PermissionStatus.Granted) statusphoto = await Permissions.RequestAsync<Permissions.Photos>();
            if (statusphoto == PermissionStatus.Granted)
            {
                var statusmedia = await Permissions.CheckStatusAsync<Permissions.Media>();
                if (statusmedia != PermissionStatus.Granted) statusmedia = await Permissions.RequestAsync<Permissions.Media>();
                if (statusmedia != PermissionStatus.Granted) return 5;
                else
                {
                    var media = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Large, ////Resize to 75% of original
                        CompressionQuality = 100,
                    });

                    if (media == null) return 2;
                    else
                    {
                        Picture = converter.GetImageBytes(media.GetStream());
                        return 1;
                    }
                }
            }
            else return 4;
        }

        public int UpdateSetting()
        {
            var camelcase = new CamelCaseChecker();
            var fname = camelcase.CapitalizeWord(Firstname);
            var lname = camelcase.CapitalizeWord(Lastname);
            Firstname = fname;
            Lastname = lname;

            var userid = Application.Current.Properties["current_login"] as string;
            var model = new Clients
            {
                Id = userid,
                Firstname = fname,
                Lastname = lname,
                Username = Username,
                Password = Password,
                Description = Description,
                ProfileImage = Picture,
                AppTheme = Theme,
                AccountCreated = new DateTime(),
                AccountUpdated = DateTime.Now,
                Logout = new DateTime(),
                IsLogin = true,
                HaveOnetimeLogin = true,
                HaveUpdated = true,
                Income = 0,
                TotalCommitment = 0,
                NetIncome = 0
            };

            string getuser = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
            var user = connection.Query<Clients>(getuser).ToList();
            if (user.Count > 0)
            {
                model.Logout = user[0].Logout;
                model.HaveOnetimeLogin = user[0].HaveOnetimeLogin;
                model.AccountCreated = user[0].AccountCreated;
                model.Income = user[0].Income;
                model.NetIncome = user[0].NetIncome;
                model.TotalCommitment = user[0].TotalCommitment;
            }

            string query = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0)
            {
                var search = "SELECT * FROM Clients WHERE Username = '" + Username + "' AND Id NOT IN ('" + result[0].Id + "')";
                var exist = connection.Query<Clients>(search).ToList();
                if (exist.Count > 0) return 4;
                else
                {
                    var row = connection.Update(model);
                    if (row == 1)
                    {
                        var updated = connection.Query<Clients>("SELECT * FROM Clients WHERE Id = '" + userid + "'").ToList();
                        if (updated.Count > 0)
                        {
                            Username = updated[0].Username;
                            Firstname = updated[0].Firstname;
                            Lastname = updated[0].Lastname;
                            Password = updated[0].Password;
                            Description = updated[0].Description;
                            Theme = updated[0].AppTheme;
                            Picture = updated[0].ProfileImage;
                            MessagingCenter.Send(this, "AppThemeUpdated");
                        }
                        return 1;
                    }
                    else return 3;
                }
            }
            else return 2;
        }

        public async Task<int> UpdateAppTheme()
        {
            try
            {
                var userid = Application.Current.Properties["current_login"] as string;
                string query = "UPDATE Clients SET AppTheme = '" + Theme + "' WHERE Id = '" + userid + "'";
                connection.Query<Clients>(query);

                Application.Current.Properties["app_theme"] = Theme;
                await Application.Current.SavePropertiesAsync();

                try { MessagingCenter.Send(this, "AppThemeUpdated"); }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    var stack = ex.StackTrace;
                    var page = Application.Current.MainPage;
                    await page.DisplayAlert(error, stack, "OK");
                    return 2;
                }
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var stack = ex.StackTrace;
                var page = Application.Current.MainPage;
                await page.DisplayAlert(error, stack, "OK");
                return 2;
            }
        }
    }
}
