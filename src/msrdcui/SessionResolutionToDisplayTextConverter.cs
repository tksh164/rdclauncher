using System;
using System.Globalization;
using System.Windows.Data;

namespace rdclauncher
{
    [ValueConversion(typeof(SessionsResolution), typeof(string))]
    public class SessionResolutionToDisplayTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sessionResolution = (SessionsResolution)value;
            return sessionResolution.ResolutionWidth != SessionsResolution.UnsetResolutionValue && sessionResolution.ResolutionHeight != SessionsResolution.UnsetResolutionValue
                ? string.Format("{0} x {1}", sessionResolution.ResolutionWidth, sessionResolution.ResolutionHeight)
                : "Match the local screen";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
