using System;
using Xamarin.Forms;

namespace Xperimen.Helper
{
    class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var text = "--";
            if (value == null) text = "--";
            if (value.Equals("ms")) text = "Bahasa Malaysia";
            if (value.Equals("en")) text = "English";
            if (value.Equals("fil")) text = "Pilipino";
            if (value.Equals("hi")) text = "हिन्दी";
            if (value.Equals("ta")) text = "तामिल";
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
