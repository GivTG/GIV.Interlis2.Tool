using System.Windows;
using System.Windows.Data;

namespace GIV.Interlis2.Tools.App.Converters
{
    public sealed class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object paramter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if ((bool)value)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // do nothing
            return false;
        }
    }
}
