using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xperimen.Helper
{
    class DatetimeNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string final;
            try
            {
                if (value == null) final = "--";
                else
                {
                    DateTime dt;
                    if (DateTime.TryParse(value.ToString(), out dt))
                    {
                        if (dt.Day == 1 && dt.Month == 1 && dt.Year == 0001) 
                            final = "--";
                        else
                            final = dt.ToString("MMM d, yyyy");
                    }
                    else final = "--";
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                final = "--";
            }
            return final;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
