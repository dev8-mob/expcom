using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel.Gallery
{
    public class GalleryViewmodel : BaseViewModel
    {
        #region properties
        bool _hasmedia;
        bool _nomedia;
        DateTime _mindt;
        DateTime _maxdt;
        bool _haveDateRange;
        ObservableCollection<SelfCommitment> _imagecommlist;
        ObservableCollection<Expenses> _imageexplist;
        bool _hascommitmentdoneshowbadge;
        int _commitmentnotdone;
        public bool HasMedia
        {
            get { return _hasmedia; }
            set { _hasmedia = value; OnPropertyChanged(); }
        }
        public bool NoMedia
        {
            get { return _nomedia; }
            set { _nomedia = value; OnPropertyChanged(); }
        }
        public bool HaveDateRange
        {
            get { return _haveDateRange; }
            set { _haveDateRange = value; OnPropertyChanged(); }
        }
        public ObservableCollection<SelfCommitment> ImageCommList
        {
            get { return _imagecommlist; }
            set { _imagecommlist = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Expenses> ImageExpList
        {
            get { return _imageexplist; }
            set { _imageexplist = value; OnPropertyChanged(); }
        }
        public DateTime MinDt
        {
            get { return _mindt; }
            set { _mindt = value; OnPropertyChanged(); }
        }
        public DateTime MaxDt
        {
            get { return _maxdt; }
            set { _maxdt = value; OnPropertyChanged(); }
        }
        public bool HasCommitmentDoneShowBadge
        {
            get { return _hascommitmentdoneshowbadge; }
            set { _hascommitmentdoneshowbadge = value; OnPropertyChanged(); }
        }
        public int CommitmentNotDone
        {
            get { return _commitmentnotdone; }
            set { _commitmentnotdone = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;
        public string userid;

        public GalleryViewmodel()
        {
            HasMedia = false;
            NoMedia = false;
            ImageCommList = new ObservableCollection<SelfCommitment>();
            ImageExpList = new ObservableCollection<Expenses>();
            MinDt = DateTime.Now;
            MaxDt = DateTime.Now;
            HaveDateRange = false;
            HasCommitmentDoneShowBadge = false;
            CommitmentNotDone = 0;
            connection = new SQLiteConnection(App.DB_PATH);
            userid = string.Empty;
            SetupData();
        }

        public void SetupData()
        {
            if (Application.Current.Properties.ContainsKey("current_login"))
                userid = Application.Current.Properties["current_login"] as string;
            GetAllMedia();
            GetCommitmentList();
        }

        public int GetAllMedia()
        {
            HasMedia = false; NoMedia = false; HaveDateRange = false;
            ImageCommList = new ObservableCollection<SelfCommitment>();
            ImageExpList = new ObservableCollection<Expenses>();
            try
            {
                string querycomm = "SELECT * FROM SelfCommitment WHERE Userid = '" + userid + "'";
                string queryexp = "SELECT * FROM Expenses WHERE Userid = '" + userid + "'";
                var listcomm = connection.Query<SelfCommitment>(querycomm).ToList();
                var listexp = connection.Query<Expenses>(queryexp).ToList();

                if (listcomm.Count > 0)
                {
                    foreach (var data in listcomm)
                        if (data.Picture != null)
                            ImageCommList.Add(data);
                }
                if (listexp.Count > 0)
                {
                    foreach (var data in listexp)
                        if (data.Picture != null)
                            ImageExpList.Add(data);
                }
                if (ImageCommList.Count == 0 && ImageExpList.Count == 0) { HasMedia = false; NoMedia = true;}
                else if (ImageCommList.Count > 0 || ImageExpList.Count > 0) 
                { 
                    HasMedia = true; NoMedia = false;
                    if (ImageExpList.Count > 0)
                    {
                        if (ImageExpList.Count == 1) 
                        { MaxDt = ImageExpList[0].ExpensesDt; }
                        else
                        {
                            foreach (var item in ImageExpList)
                            {
                                if (item.ExpensesDt < MinDt) MinDt = item.ExpensesDt;
                                if (item.ExpensesDt > MaxDt) MaxDt = item.ExpensesDt;
                            }
                            var remin = new DateTime(MinDt.Year, MinDt.Month, MinDt.Day);
                            var remax = new DateTime(MaxDt.Year, MaxDt.Month, MaxDt.Day);
                            var diff = remax - remin;
                            if (diff.Days == 0) { HaveDateRange = false; }
                            else if (diff.Days > 0) { HaveDateRange = true; }
                        }
                    }
                }
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
                CommitmentNotDone = 0;
                string query = "SELECT * FROM SelfCommitment WHERE Userid = '" + userid + "'";
                var ListCommitments = connection.Query<SelfCommitment>(query).ToList();

                if (ListCommitments.Count > 0)
                {
                    var checkdone = 0;
                    foreach (var data in ListCommitments)
                    {
                        if (!data.IsDone) CommitmentNotDone++;
                        if (data.IsDone) checkdone++;
                    }
                    if (CommitmentNotDone > 0) HasCommitmentDoneShowBadge = true;
                    if (checkdone == ListCommitments.Count) HasCommitmentDoneShowBadge = false;
                }
                else HasCommitmentDoneShowBadge = false;
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
