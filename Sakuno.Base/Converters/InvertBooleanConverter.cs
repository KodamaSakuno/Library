using System;
using System.Globalization;
using System.Windows.Data;
using ConvertType = System.Convert;

namespace Sakuno.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture) => !ConvertType.ToBoolean(rpValue);

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture) => !ConvertType.ToBoolean(rpValue);
    }
}
