using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xperimen.Helper;
using Xperimen.Model;

namespace Xperimen.ViewModel.Commitment
{
    public class CommitmentViewmodel : BaseViewModel
    {
        #region bindable properties
        bool _norecord;
        bool _hasrecord;
        string _title;
        string _description;
        double _amount;
        bool _isdone;
        bool _hasaccno;
        bool _hasattachment;
        int _accountno;
        MediaFile _picture;
        List<SelfCommitment> _listcommitments;
        double _income;
        double _totalcommitment;
        double _balance;
        byte[] _profilepic;
        int _notyetpaid;
        DateTime _currentdt;
        DateTime _upcomingdt;
        bool _alldone;
        bool _allnotdone;
        public bool NoCommitment
        {
            get { return _norecord; }
            set { _norecord = value; OnPropertyChanged(); }
        }
        public bool HasCommitment
        {
            get { return _hasrecord; }
            set { _hasrecord = value; OnPropertyChanged(); }
        }
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
            set
            {
                _hasaccno = value;
                OnPropertyChanged();
                AccountNo = HasAccNo ? AccountNo : 0;
            }
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
        public List<SelfCommitment> ListCommitments
        {
            get { return _listcommitments; }
            set { _listcommitments = value; OnPropertyChanged(); }
        }
        public double Income
        {
            get { return _income; }
            set { _income = value; OnPropertyChanged(); }
        }
        public double TotalCommitment
        {
            get { return _totalcommitment; }
            set { _totalcommitment = value; OnPropertyChanged(); }
        }
        public double Balance
        {
            get { return _balance; }
            set { _balance = value; OnPropertyChanged(); }
        }
        public byte[] ProfilePic
        {
            get { return _profilepic; }
            set { _profilepic = value; OnPropertyChanged(); }
        }
        public int NotYetPaid
        {
            get { return _notyetpaid; }
            set { _notyetpaid = value; OnPropertyChanged(); }
        }
        public DateTime CurrentDt
        {
            get { return _currentdt; }
            set { _currentdt = value; OnPropertyChanged(); }
        }
        public DateTime UpcomingDt
        {
            get { return _upcomingdt; }
            set { _upcomingdt = value; OnPropertyChanged(); }
        }
        public bool AllCommitmentDone
        {
            get { return _alldone; }
            set { _alldone = value; OnPropertyChanged(); }
        }
        public bool AllCommitmentNotDone
        {
            get { return _allnotdone; }
            set { _allnotdone = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;

        public CommitmentViewmodel()
        {
            connection = new SQLiteConnection(App.DB_PATH);
            NoCommitment = true;
            HasCommitment = false;
            Title = string.Empty;
            Description = string.Empty;
            Amount = 0;
            IsDone = false;
            HasAccNo = false;
            HasAttachment = false;
            AccountNo = 0;
            Picture = null;
            Income = 0;
            TotalCommitment = 0;
            Balance = 0;
            NotYetPaid = 0;
            CurrentDt = DateTime.Now;
            UpcomingDt = DateTime.Now.AddMonths(1);
            AllCommitmentDone = false;
            AllCommitmentNotDone = true;

            var userid = Application.Current.Properties["current_login"] as string;
            string query = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0) ProfilePic = result[0].ProfileImage;
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
                var userid = string.Empty;
                if (Application.Current.Properties.ContainsKey("current_login"))
                    userid = Application.Current.Properties["current_login"] as string;

                var camelcase = new CamelCaseChecker();
                var title = camelcase.CapitalizeWord(Title);
                var convert = new StreamByteConverter();
                var data = new SelfCommitment
                {
                    Id = Guid.NewGuid().ToString(),
                    Userid = userid,
                    Title = title,
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

        public int GetCommitmentList()
        {
            try
            {
                var userid = string.Empty;
                if (Application.Current.Properties.ContainsKey("current_login"))
                    userid = Application.Current.Properties["current_login"] as string;

                string getuser = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
                var user = connection.Query<Clients>(getuser).ToList();
                if (user.Count > 0) Income = user[0].Income;

                TotalCommitment = 0; NotYetPaid = 0;
                ListCommitments = new List<SelfCommitment>();
                string query = "SELECT * FROM SelfCommitment WHERE Userid = '" + userid + "'";
                ListCommitments = connection.Query<SelfCommitment>(query).ToList();

                if (ListCommitments.Count > 0)
                {
                    NoCommitment = false;
                    HasCommitment = true;
                    foreach (var data in ListCommitments)
                    {
                        TotalCommitment += data.Amount;
                        if (!data.IsDone) NotYetPaid++;
                    }

                    if (NotYetPaid == 0)
                    { AllCommitmentDone = true; AllCommitmentNotDone = false; }
                    else if (NotYetPaid > 0)
                    { AllCommitmentDone = false; AllCommitmentNotDone = true; }
                }
                else
                { NoCommitment = true; HasCommitment = false; }

                Balance = Income - TotalCommitment;
                return SaveNetBalance();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 3;
            }
        }

        public int SaveNetBalance()
        {
            try
            {
                var userid = string.Empty;
                if (Application.Current.Properties.ContainsKey("current_login"))
                    userid = Application.Current.Properties["current_login"] as string;

                var query = "UPDATE Clients SET NetIncome = " + Balance + " WHERE Id = '" + userid + "'";
                connection.Query<Clients>(query);
                var cek = connection.Query<Clients>("SELECT * FROM Clients WHERE Id = '" + userid + "'").ToList();
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public int SetDataCommitment(string data)
        {
            try
            {
                var query = "SELECT * FROM SelfCommitment WHERE Id = '" + data + "'";
                var result = connection.Query<SelfCommitment>(query).ToList();
                if (result.Count > 0)
                {
                    Title = result[0].Title;
                    Description = result[0].Description;
                    Amount = result[0].Amount;
                    IsDone = result[0].IsDone;
                    HasAccNo = result[0].HasAccNo;
                    HasAttachment = result[0].HasAttachment;
                    AccountNo = result[0].AccountNo;
                    ProfilePic = result[0].Picture;
                    return 1;
                }
                return 2;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 3;
            }
        }

        public int UpdateCommitment(string data)
        {
            try
            {
                var camelcase = new CamelCaseChecker();
                var title = camelcase.CapitalizeWord(Title);
                Title = title;

                if (Picture != null)
                {
                    var userid = string.Empty;
                    if (Application.Current.Properties.ContainsKey("current_login"))
                        userid = Application.Current.Properties["current_login"] as string;

                    var model = new SelfCommitment
                    {
                        Id = data,
                        Userid = userid,
                        Title = Title,
                        Description = Description,
                        Amount = Amount,
                        IsDone = IsDone,
                        HasAccNo = HasAccNo,
                        HasAttachment = HasAttachment,
                        AccountNo = AccountNo,
                        Picture = null
                    };

                    var convert = new StreamByteConverter();
                    model.Picture = convert.GetImageBytes(Picture.GetStream());
                    connection.Query<SelfCommitment>("DELETE FROM SelfCommitment WHERE Id = '" + data + "'");
                    connection.Insert(model);
                }
                else
                {
                    var query = "UPDATE SelfCommitment SET Title = '" + title + "', Description = '" + Description + "', Amount = "
                    + Amount + ", IsDone = " + IsDone + ", HasAccNo = " + HasAccNo + ", HasAttachment = " + HasAttachment
                    + ", AccountNo = " + AccountNo + ", Picture = NULL WHERE Id = '" + data + "'";
                    connection.Query<SelfCommitment>(query);
                }

                var cek = connection.Query<SelfCommitment>("SELECT * FROM SelfCommitment WHERE Id = '" + data + "'").ToList();
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public int DeleteCommitment(string data)
        {
            try
            {
                connection.Query<SelfCommitment>("DELETE FROM SelfCommitment WHERE Id = '" + data + "'");
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public int SetStatusDonePaid(string data, bool status)
        {
            try
            {
                string query = "UPDATE SelfCommitment SET IsDone = " + status + " WHERE Id = '" + data + "'";
                connection.Query<SelfCommitment>(query);
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public int SaveIncome()
        {
            try
            {
                var userid = string.Empty;
                if (Application.Current.Properties.ContainsKey("current_login"))
                    userid = Application.Current.Properties["current_login"] as string;

                string query = "UPDATE Clients SET Income = " + Income + " WHERE Id = '" + userid + "'";
                connection.Query<Clients>(query);
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
