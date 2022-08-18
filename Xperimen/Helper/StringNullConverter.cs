using System;
using Xamarin.Forms;

namespace Xperimen.Helper
{
    class StringNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var text = "--";
            if (value == null) text = "--";
            if (value != null)
                text = value.ToString();
            if (text.Contains("NULL")) text = text.ToString().Replace("NULL", "--");
            if (text == string.Empty) text = "--";
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
