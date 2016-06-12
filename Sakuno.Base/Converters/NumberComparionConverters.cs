using System;
using System.Globalization;
using System.Windows.Data;
using ConvertType = System.Convert;

namespace Sakuno.Converters
{
    public abstract class NumberComparionConverter : IValueConverter
    {
        Func<double, double, bool> r_Func;

        protected NumberComparionConverter(Func<double, double, bool> rpFunc)
        {
            r_Func = rpFunc;
        }

        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            var rValue = ConvertType.ToDouble(rpValue);
            var rParameter = ConvertType.ToDouble(rpParameter);

            return r_Func(rValue, rParameter);
        }

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }

    public class IsGreaterThanConverter : NumberComparionConverter
    {
        public IsGreaterThanConverter() : base((x, y) => x > y) { }
    }
    public class IsGreaterThanOrEqualToConverter : NumberComparionConverter
    {
        public IsGreaterThanOrEqualToConverter() : base((x, y) => x >= y) { }
    }

    public class IsLessThanConverter : NumberComparionConverter
    {
        public IsLessThanConverter() : base((x, y) => x < y) { }
    }
    public class IsLessThanOrEqualToConverter : NumberComparionConverter
    {
        public IsLessThanOrEqualToConverter() : base((x, y) => x <= y) { }
    }
}
