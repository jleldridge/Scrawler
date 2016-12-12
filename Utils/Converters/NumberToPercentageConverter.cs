using System;
using Windows.UI.Xaml.Data;

namespace Utils.Converters
{
    public class NumberToPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is float)
            {
                return (float)value * 100;
            }
            else if (value is double)
            {
                return (double)value * 100;
            }
            else if (value is int)
            {
                return (int)value * 100;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is float)
            {
                return (float)value / 100;
            }
            else if (value is double)
            {
                return (double)value / 100;
            }
            else if (value is int)
            {
                return (int)value / 100;
            }

            return null;
        }
    }
}
