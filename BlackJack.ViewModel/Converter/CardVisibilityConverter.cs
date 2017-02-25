using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BlackJack.Model;
using BlackJack.Model.Enum;

namespace BlackJack.ViewModel.Converter
{
    public class CardVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var jackModel = value as BlackJackModel;
            if (jackModel == null) throw new Exception();

            var model = jackModel;
            if (model.Index == 0 && model.UserType == UserType.Dealer)
                return Visibility.Hidden;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
