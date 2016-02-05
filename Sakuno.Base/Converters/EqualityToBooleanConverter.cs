using System;
using System.Globalization;
using System.Windows.Data;

namespace Sakuno.Converters
{
    public class EqualityToBooleanConverter : IValueConverter
    {
        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture) => rpValue.Equals(rpParameter);

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }
}
