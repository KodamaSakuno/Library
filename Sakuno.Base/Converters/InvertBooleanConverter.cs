using System;
using System.Globalization;
using System.Windows.Data;

namespace Sakuno.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture) => !(bool)rpValue;

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }
}
