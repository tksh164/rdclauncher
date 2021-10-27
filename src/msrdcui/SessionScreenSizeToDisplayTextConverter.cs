using System;
using System.Globalization;
using System.Windows.Data;

namespace rdclauncher
{
    [ValueConversion(typeof(SessionScreenSize), typeof(string))]
    public class SessionScreenSizeToDisplayTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sessionScreenSize = (SessionScreenSize)value;
            return sessionScreenSize.ResolutionWidth != SessionScreenSize.UnsetResolutionValue && sessionScreenSize.ResolutionHeight != SessionScreenSize.UnsetResolutionValue
                ? string.Format("{0} x {1}", sessionScreenSize.ResolutionWidth, sessionScreenSize.ResolutionHeight)
                : "Match the local screen";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
