using System;
using System.Globalization;
using System.Windows.Data;

namespace FaceGame.Converters
{
    public class ScoreToColour:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var score = (int)value;
            if (score >= 0)
                return "#1DD300";
            else
                return "#FF5300";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}