
using System.Collections.ObjectModel;
using Xperimen.Model;

namespace Xperimen.ViewModel.Setting
{
    public class CurrencyViewmodel : BaseViewModel
    {
        #region bindable properties
        ObservableCollection<Currency> _listcurrency;
        public ObservableCollection<Currency> ListCurrency
        {
            get => _listcurrency;
            set { SetProperty(ref _listcurrency, value); }
        }
        #endregion

        public CurrencyViewmodel()
        {
            ListCurrency = new ObservableCollection<Currency>();
            SetupData();
        }

        public void SetupData()
        {
            ListCurrency.Add(new Currency { Code = "IDR", Name = "Indonesian Rupiah", Shortform = "RP" });
            ListCurrency.Add(new Currency { Code = "MYR", Name = "Malaysian Ringgit", Shortform = "RM" });
            ListCurrency.Add(new Currency { Code = "PHP", Name = "Philippine Peso", Shortform = "₱" });
            ListCurrency.Add(new Currency { Code = "SGD", Name = "Singapore Dollar", Shortform = "S$" });
            ListCurrency.Add(new Currency { Code = "USD", Name = "US Dollar", Shortform = "$" });
        }
    }
}
