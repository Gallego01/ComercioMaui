using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ComercioMaui.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDeleted = (bool)value;
            return isDeleted ? Colors.Green : Colors.Red; // verde = habilitar, rojo = deshabilitar
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
