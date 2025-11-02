using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace ComercioMaui.Converters
{
    public class BoolToDisabledTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDeleted = (bool)value;
            // Texto gris si está deshabilitada, negro si está activa
            return isDeleted ? Colors.Gray : Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
