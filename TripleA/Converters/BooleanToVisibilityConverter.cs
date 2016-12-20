using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TripleA.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool booleanValue = (bool)value;

            // flip it!
            if (parameter != null && parameter.Equals("INVERT"))
            {
                booleanValue = !booleanValue;
            }

            if (booleanValue)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}