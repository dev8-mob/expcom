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

namespace Xperimen.ViewModel.Expense
{
    public class ExpensesViewmodel : BaseViewModel
    {
        #region bindable properties
        double _amount;
        string _title;
        MediaFile _picture;
        List<Expenses> _listexpenses;
        double _total;
        bool _noexpenses;
        bool _hasexpenses;
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
        public MediaFile Picture
        {
            get { return _picture; }
            set { _picture = value; OnPropertyChanged(); }
        }
        public List<Expenses> ListExpenses
        {
            get { return _listexpenses; }
            set { _listexpenses = value; OnPropertyChanged(); }
        }
        public double TotalExpenses
        {
            get { return _total; }
            set { _total = value; OnPropertyChanged(); }
        }
        public bool NoExpenses
        {
            get { return _noexpenses; }
            set { _noexpenses = value; OnPropertyChanged(); }
        }
        public bool HasExpenses
        {
            get { return _hasexpenses; }
            set { _hasexpenses = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;

        public ExpensesViewmodel()
        {
            connection = new SQLiteConnection(App.DB_PATH);
            Amount = 0;
            Title = string.Empty;
            Picture = null;
            ListExpenses = new List<Expenses>();
            TotalExpenses = 0;
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

        public int GetExpensesList()
        {
            try
            {
                var userid = string.Empty;
                if (Application.Current.Properties.ContainsKey("current_login"))
                    userid = Application.Current.Properties["current_login"] as string;

                string getuser = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
                var user = connection.Query<Clients>(getuser).ToList();
                //if (user.Count > 0) Income = user[0].Income;

                TotalExpenses = 0;
                ListExpenses = new List<Expenses>();
                string query = "SELECT * FROM Expenses WHERE Userid = '" + userid + "'";
                ListExpenses = connection.Query<Expenses>(query).ToList();

                if (ListExpenses.Count > 0)
                {
                    NoExpenses = false;
                    HasExpenses = true;
                    foreach (var data in ListExpenses)
                    {
                        TotalExpenses += data.Amount;
                    }
                }
                else
                { NoExpenses = true; HasExpenses = false; }
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 3;
            }
        }
    }
}
