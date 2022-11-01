
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel.Setting
{
    public class CurrencyViewmodel : BaseViewModel
    {
        #region bindable properties
        public string _currency;
        ObservableCollection<Currency> _listcurrency;
        public string Currency
        {
            get => _currency;
            set { SetProperty(ref _currency, value); }
        }
        public ObservableCollection<Currency> ListCurrency
        {
            get => _listcurrency;
            set { SetProperty(ref _listcurrency, value); }
        }
        #endregion

        public SQLiteConnection connection;

        public CurrencyViewmodel()
        {
            Currency = string.Empty;
            ListCurrency = new ObservableCollection<Currency>();
            connection = new SQLiteConnection(App.DB_PATH);
            SetupData();
        }

        public void SetupData()
        {
            var userid = Application.Current.Properties["current_login"] as string;
            string query = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0) Currency = result[0].Currency;

            ListCurrency.Add(new Currency { Code = "IDR", Name = "Indonesian Rupiah", Shortform = "RP", IsSelected = false });
            ListCurrency.Add(new Currency { Code = "MYR", Name = "Malaysian Ringgit", Shortform = "RM", IsSelected = false });
            ListCurrency.Add(new Currency { Code = "PHP", Name = "Philippine Peso", Shortform = "₱", IsSelected = false });
            ListCurrency.Add(new Currency { Code = "SGD", Name = "Singapore Dollar", Shortform = "S$", IsSelected = false });
            ListCurrency.Add(new Currency { Code = "USD", Name = "US Dollar", Shortform = "$", IsSelected = false });

            foreach (var item in ListCurrency)
            {
                if (item.Code.Equals(Currency))
                    item.IsSelected = true;
            }
        }

        public int UpdateCurrency()
        {
            try
            {
                var userid = Application.Current.Properties["current_login"] as string;
                var query = "UPDATE Clients SET Currency = '" + Currency + "' WHERE Id = '" + userid + "'";
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
    }
}
