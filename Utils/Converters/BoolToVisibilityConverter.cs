using System;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Utils.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = value as bool?;
            return (!Invert && boolValue == true) || (Invert && boolValue == false) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, string language)
        {
            throw new System.NotImplementedException();
        }
    }
}
