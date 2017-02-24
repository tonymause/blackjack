using System;
using System.Globalization;
using System.Windows.Data;

namespace BlackJack.ViewModel.Converter
{
    public class NumberToSuitDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ret;

            if (!(value is int)) throw new Exception();

            var v = (int) value;
            switch (v)
            {
                case 11:
                    ret = "J";
                    break;
                case 12:
                    ret = "Q";
                    break;
                case 13:
                    ret = "K";
                    break;
                default:
                    ret = v.ToString();
                    break;
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
