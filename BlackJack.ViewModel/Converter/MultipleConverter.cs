using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BlackJack.Model;

namespace BlackJack.ViewModel.Converter
{
    public class MultipleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var jackModel = values[0] as BlackJackModel;
            var started = values[1] as bool?;

            if (jackModel == null || started == null) throw new Exception();

            if (jackModel.Index == 0 && jackModel.UserType == Model.Enum.UserType.Dealer && started.Value)
            {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
