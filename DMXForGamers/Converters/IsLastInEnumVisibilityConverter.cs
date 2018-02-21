using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace DMXForGamers.Converters
{
    public class IsLastInEnumVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be of type 'Visibility'");

            if(values.Count() != 2)
                throw new InvalidOperationException("Two values are expected (IEnumerable<T> and object)");

            if (values[0] != null)
            {
                if (values[0].GetType().GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)) == false)
                    throw new InvalidOperationException("First value must be an enumeration");

                var objEnum = (values[0] as IEnumerable).Cast<object>();
                var obj = values[1];

                return ((objEnum.Last() == obj) ? Visibility.Visible : Visibility.Hidden);
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
