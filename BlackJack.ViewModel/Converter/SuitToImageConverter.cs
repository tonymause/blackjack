using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BlackJack.Model.Enum;

namespace BlackJack.ViewModel.Converter
{
    public class SuitToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((CardType)value)
            {
                case CardType.Club:
                    return ConvertToBitMapImage("../Images/Club.png");
                case CardType.Diamond:
                    return ConvertToBitMapImage("../Images/Diamond.png");
                case CardType.Heart:
                    return ConvertToBitMapImage("../Images/Heart.png");
                case CardType.Spade:
                    return ConvertToBitMapImage("../Images/Spade.png");

                default:
                    return null;
            }
        }

        private BitmapImage ConvertToBitMapImage(string uri)
        {
            var img = new BitmapImage();

            img.BeginInit();
            img.UriSource = new Uri(uri, UriKind.Relative);
            img.EndInit();

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
