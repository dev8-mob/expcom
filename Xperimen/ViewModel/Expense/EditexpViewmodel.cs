using Plugin.Media.Abstractions;
using Plugin.Media;
using SQLite;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xperimen.Model;
using Xperimen.Helper;

namespace Xperimen.ViewModel.Expense
{
    public class EditexpViewmodel : BaseViewModel
    {
        #region bindable properties
        double _amount;
        string _title;
        bool _hasattachment;
        DateTime _datetimeexp;
        string _expensedatetime;
        byte[] _picture;
        MediaFile _picturemedia;
        public double Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged(); }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        public bool HasAttachment
        {
            get { return _hasattachment; }
            set { _hasattachment = value; OnPropertyChanged(); }
        }
        public DateTime ExpensesDt
        {
            get { return _datetimeexp; }
            set { _datetimeexp = value; OnPropertyChanged(); }
        }
        public string ExpenseDateTime
        {
            get { return _expensedatetime; }
            set { _expensedatetime = value; OnPropertyChanged(); }
        }
        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; OnPropertyChanged(); }
        }
        public MediaFile PictureMedia
        {
            get { return _picturemedia; }
            set { _picturemedia = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;
        public string id;

        public EditexpViewmodel(string id)
        {
            connection = new SQLiteConnection(App.DB_PATH);
            this.id = id;
            Amount = 0;
            Title = string.Empty;
            HasAttachment = false;
            ExpensesDt = DateTime.Now;
            ExpenseDateTime = string.Empty;
            Picture = null;
            PictureMedia = null;
            SetupView();
        }

        public void SetupView()
        {
            var query = "SELECT * FROM Expenses WHERE Id = '" + id + "'";
            var result = connection.Query<Expenses>(query).ToList();
            if (result.Count > 0)
            {
                Amount = result[0].Amount;
                Title = result[0].Title;
                HasAttachment = result[0].HasAttachment;
                ExpensesDt = result[0].ExpensesDt;
                ExpenseDateTime = result[0].ExpenseDateTime;
                Picture = result[0].Picture;
            }
        }

        public async Task<int> TakePhoto()
        {
            PictureMedia = null;
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) return 3;
            PictureMedia = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Xperimen",
                PhotoSize = PhotoSize.Large, ////Resize to 75% of original
                CompressionQuality = 100,
                Name = Guid.NewGuid().ToString().Substring(0, 10) + ".jpg",
                SaveToAlbum = true
                //CustomPhotoSize = 90, //Resize to 90% of original
            });

            if (PictureMedia == null) return 2;
            else return 1;
        }

        public async Task<int> PickPhoto()
        {
            PictureMedia = null;
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported || !CrossMedia.Current.IsPickVideoSupported) return 3;
            PictureMedia = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Large, ////Resize to 75% of original
                CompressionQuality = 100,
            });

            if (PictureMedia == null) return 2;
            else return 1;
        }

        public int UpdateExpenses()
        {
            try
            {
                var camelcase = new CamelCaseChecker();
                var title = camelcase.CapitalizeWord(Title);
                Title = title;

                var userid = string.Empty;
                if (Application.Current.Properties.ContainsKey("current_login"))
                    userid = Application.Current.Properties["current_login"] as string;

                var data = new Expenses
                {
                    Id = id,
                    Userid = userid,
                    Amount = Amount,
                    Title = title,
                    HasAttachment = HasAttachment,
                    ExpensesDt = ExpensesDt,
                    ExpenseDateTime = ExpenseDateTime,
                    Picture = null
                };

                if (PictureMedia != null)
                {
                    var convert = new StreamByteConverter();
                    data.Picture = convert.GetImageBytes(PictureMedia.GetStream());
                }
                else data.Picture = Picture;

                connection.Query<SelfCommitment>("DELETE FROM Expenses WHERE Id = '" + id + "'");
                connection.Insert(data);
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
