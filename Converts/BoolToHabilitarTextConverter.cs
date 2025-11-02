using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ComercioMaui.Converters
{
    public class BoolToHabilitarTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDeleted = (bool)value;
            return isDeleted ? "Habilitar" : "Deshabilitar";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
