using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Threading.Tasks;
using Xperimen.Helper;
using Xperimen.Model;

namespace Xperimen.ViewModel.Commitment
{
    public class CommitmentViewmodel : BaseViewModel
    {
        #region properties
        string _title;
        string _description;
        double _amount;
        bool _isdone;
        bool _hasaccno;
        bool _hasattachment;
        int _accountno;
        MediaFile _picture;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        public double Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged(); }
        }
        public bool IsDone
        {
            get { return _isdone; }
            set { _isdone = value; OnPropertyChanged(); }
        }
        public bool HasAccNo
        {
            get { return _hasaccno; }
            set { _hasaccno = value; OnPropertyChanged(); }
        }
        public bool HasAttachment
        {
            get { return _hasattachment; }
            set { _hasattachment = value; OnPropertyChanged(); }
        }
        public int AccountNo
        {
            get { return _accountno; }
            set { _accountno = value; OnPropertyChanged(); }
        }
        public MediaFile Picture
        {
            get { return _picture; }
            set { _picture = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;

        public CommitmentViewmodel()
        {
            Title = string.Empty;
            Description = string.Empty;
            Amount = 0;
            IsDone = false;
            HasAccNo = false;
            HasAttachment = false;
            AccountNo = 0;
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

        public int AddCommitment()
        {
            try
            {
                var convert = new StreamByteConverter();
                var data = new SelfCommitment
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = Title,
                    Description = Description,
                    Amount = Amount,
                    IsDone = false,
                    HasAccNo = HasAccNo,
                    HasAttachment = HasAttachment,
                    AccountNo = AccountNo,
                    Picture = null
                };
                if (Picture != null) data.Picture = convert.GetImageBytes(Picture.GetStream());
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
