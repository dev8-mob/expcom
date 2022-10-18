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
        ObservableCollection<byte[]> _imagecommlist;
        ObservableCollection<byte[]> _imageexplist;
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
        public ObservableCollection<byte[]> ImageCommList
        {
            get { return _imagecommlist; }
            set { _imagecommlist = value; OnPropertyChanged(); }
        }
        public ObservableCollection<byte[]> ImageExpList
        {
            get { return _imageexplist; }
            set { _imageexplist = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;
        public string userid;

        public GalleryViewmodel()
        {
            HasMedia = false;
            NoMedia = false;
            ImageCommList = new ObservableCollection<byte[]>();
            ImageExpList = new ObservableCollection<byte[]>();
            connection = new SQLiteConnection(App.DB_PATH);
            userid = string.Empty;
            SetupData();
        }

        public void SetupData()
        {
            if (Application.Current.Properties.ContainsKey("current_login"))
                userid = Application.Current.Properties["current_login"] as string;
            GetAllMedia();
        }

        public int GetAllMedia()
        {
            HasMedia = false; NoMedia = false;
            ImageCommList = new ObservableCollection<byte[]>();
            ImageExpList = new ObservableCollection<byte[]>();
            try
            {
                string querycomm = "SELECT * FROM SelfCommitment WHERE Userid = '" + userid + "'";
                string queryexp = "SELECT * FROM Expenses WHERE Userid = '" + userid + "'";
                var listcomm = connection.Query<SelfCommitment>(querycomm).ToList();
                var listexp = connection.Query<Expenses>(queryexp).ToList();

                if (listcomm.Count == 0 && listexp.Count == 0) { HasMedia = false; NoMedia = true; }
                else { HasMedia = true; NoMedia = false; }
                if (listcomm.Count > 0)
                {
                    foreach (var data in listcomm)
                        if (data.Picture != null)
                            ImageCommList.Add(data.Picture);
                }
                if (listexp.Count > 0)
                {
                    foreach (var data in listexp)
                        if (data.Picture != null)
                            ImageExpList.Add(data.Picture);
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
    }
}
