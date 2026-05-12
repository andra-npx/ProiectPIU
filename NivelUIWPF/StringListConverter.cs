using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace NivelUIWPF
{
    public class StringListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> lista && lista.Count > 0)
                return string.Join(", ", lista);
            return "Niciun traseu";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}