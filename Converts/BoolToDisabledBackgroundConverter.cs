using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace ComercioMaui.Converters
{
    public class BoolToDisabledBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDeleted = (bool)value;
            // Fondo gris si está deshabilitada, blanco si está activa
            return isDeleted ? Colors.LightGray : Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
