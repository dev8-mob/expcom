using System;
using Xamarin.Forms;
using Xperimen.Resources;

namespace Xperimen.Helper
{
    class DayLanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var convert = DateTime.Parse(value.ToString());
            var text = "--";
            if (value == null) text = "--";
            if (convert.ToString("dddd").Equals("Monday")) text = AppResources.app_monday;
            if (convert.ToString("dddd").Equals("Tuesday")) text = AppResources.app_tuesday;
            if (convert.ToString("dddd").Equals("Wednesday")) text = AppResources.app_wednesday;
            if (convert.ToString("dddd").Equals("Thursday")) text = AppResources.app_thursday;
            if (convert.ToString("dddd").Equals("Friday")) text = AppResources.app_friday;
            if (convert.ToString("dddd").Equals("Saturday")) text = AppResources.app_saturday;
            if (convert.ToString("dddd").Equals("Sunday")) text = AppResources.app_sunday;
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
