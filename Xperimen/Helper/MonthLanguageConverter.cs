using System;
using Xamarin.Forms;
using Xperimen.Resources;

namespace Xperimen.Helper
{
    class MonthLanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var convert = DateTime.Parse(value.ToString());
            var text = "--";
            if (value == null) text = "--";
            if (convert.ToString("MMMM").Equals("January")) text = AppResources.app_jan;
            if (convert.ToString("MMMM").Equals("February")) text = AppResources.app_feb;
            if (convert.ToString("MMMM").Equals("March")) text = AppResources.app_mar;
            if (convert.ToString("MMMM").Equals("April")) text = AppResources.app_apr;
            if (convert.ToString("MMMM").Equals("May")) text = AppResources.app_may;
            if (convert.ToString("MMMM").Equals("June")) text = AppResources.app_jun;
            if (convert.ToString("MMMM").Equals("July")) text = AppResources.app_jul;
            if (convert.ToString("MMMM").Equals("August")) text = AppResources.app_aug;
            if (convert.ToString("MMMM").Equals("September")) text = AppResources.app_sept;
            if (convert.ToString("MMMM").Equals("October")) text = AppResources.app_oct;
            if (convert.ToString("MMMM").Equals("November")) text = AppResources.app_nov;
            if (convert.ToString("MMMM").Equals("December")) text = AppResources.app_dec;
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
