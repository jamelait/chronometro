using System;
using System.Globalization;
using System.Windows.Data;
using ChronoMetro.Business;

namespace ChronoMetro.ViewModels
{
    public class ShortTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan time = value is TimeSpan ? (TimeSpan) value : new TimeSpan();

            return Common.GetReadableTime(time.Hours, time.Minutes, time.Seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
