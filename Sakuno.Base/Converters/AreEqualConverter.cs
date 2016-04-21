using System;
using System.Globalization;
using System.Windows.Data;

namespace Sakuno.Converters
{
    public class AreEqualConverter : IMultiValueConverter
    {
        public object Convert(object[] rpValues, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            if (rpValues == null || rpValues.Length < 2)
                throw new ArgumentException(nameof(rpValues));

            return Equals(rpValues[0], rpValues[1]);
        }

        public object[] ConvertBack(object rpValue, Type[] rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }
}
