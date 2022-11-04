using System;
using Xamarin.Forms;

namespace Xperimen.Helper
{
    class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var text = "--";
            if (value == null) text = "--";
            if (value.Equals("A$")) text = "AUD - Australian Dollar";
            if (value.Equals("B$")) text = "BND - Brunei Dollar";
            if (value.Equals("¥")) text = "JPY - Japanese Yen";
            if (value.Equals("₹")) text = "INR - Indian Rupee";
            if (value.Equals("RP")) text = "IDR - Indonesian Rupiah";
            if (value.Equals("RM")) text = "MYR - Malaysian Ringgit";
            if (value.Equals("₱")) text = "PHP - Philippine Peso";
            if (value.Equals("S$")) text = "SGD - Singapore Dollar";
            if (value.Equals("$")) text = "USD - US Dollar";
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
