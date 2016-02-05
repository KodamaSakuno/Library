using System;
using System.Globalization;
using System.Windows.Data;

namespace Sakuno.Converters
{
    using ConvertClass = Convert;

    public class OppositeNumberConverter : IValueConverter
    {
        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            if (rpTargetType == typeof(double))
                return -ConvertClass.ToDouble(rpValue);

            if (rpTargetType == typeof(int))
                return -ConvertClass.ToInt32(rpValue);

            if (rpTargetType == typeof(long))
                return -ConvertClass.ToInt64(rpValue);

            if (rpTargetType == typeof(float))
                return -ConvertClass.ToSingle(rpValue);

            if (rpTargetType == typeof(short))
                return -ConvertClass.ToInt16(rpValue);

            if (rpTargetType == typeof(sbyte))
                return -ConvertClass.ToSByte(rpValue);

            return rpValue;
        }

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }
}
