using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Utils.Converters
{
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = value as Color?;
            if (color.HasValue)
            {
                return new SolidColorBrush(color.Value);
            }
            return null;
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, string language)
        {
            throw new System.NotImplementedException();
        }
    }
}
