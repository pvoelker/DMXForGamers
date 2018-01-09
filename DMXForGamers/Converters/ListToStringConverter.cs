using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace DMXForGamers.Converters
{
    [ValueConversion(typeof(IEnumerable<string>), typeof(string))]
    public class ListToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a String");

            if (value == null)
                return String.Empty;
            else
                return String.Join(Environment.NewLine, (IEnumerable<string>)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
