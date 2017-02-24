#region

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace BlackJack.ViewModel.Converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return null;

            var reverse = false;
            var isVisible = (bool) value;

            if (parameter != null)
                bool.TryParse(parameter.ToString(), out reverse);

            if (isVisible)
                return reverse ? Visibility.Collapsed : Visibility.Visible;
            return reverse ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}