using System;
using System.Globalization;
using System.Windows.Data;
using ConvertType = System.Convert;

namespace Sakuno.Converters
{
    public class IsGreaterThanConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            var rValue = ConvertType.ToDouble(rpValue);
            var rParameter = ConvertType.ToDouble(rpParameter);

            return rValue > rParameter;
        }
        public object Convert(object[] rpValues, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            if (rpValues.Length < 2)
                throw new ArgumentException();

            var rValue = ConvertType.ToDouble(rpValues[0]);
            var rParameter = ConvertType.ToDouble(rpValues[1]);

            return rValue > rParameter;
        }

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
        public object[] ConvertBack(object rpValue, Type[] rpTargetTypes, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }

    public class IsGreaterThanOrEqualToConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            var rValue = ConvertType.ToDouble(rpValue);
            var rParameter = ConvertType.ToDouble(rpParameter);

            return rValue >= rParameter;
        }
        public object Convert(object[] rpValues, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            if (rpValues.Length < 2)
                throw new ArgumentException();

            var rValue = ConvertType.ToDouble(rpValues[0]);
            var rParameter = ConvertType.ToDouble(rpValues[1]);

            return rValue >= rParameter;
        }

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
        public object[] ConvertBack(object rpValue, Type[] rpTargetTypes, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }

    public class IsLessThanConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            var rValue = ConvertType.ToDouble(rpValue);
            var rParameter = ConvertType.ToDouble(rpParameter);

            return rValue < rParameter;
        }
        public object Convert(object[] rpValues, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            if (rpValues.Length < 2)
                throw new ArgumentException();

            var rValue = ConvertType.ToDouble(rpValues[0]);
            var rParameter = ConvertType.ToDouble(rpValues[1]);

            return rValue < rParameter;
        }

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
        public object[] ConvertBack(object rpValue, Type[] rpTargetTypes, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }

    public class IsLessThanOrEqualToConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            var rValue = ConvertType.ToDouble(rpValue);
            var rParameter = ConvertType.ToDouble(rpParameter);

            return rValue <= rParameter;
        }
        public object Convert(object[] rpValues, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            if (rpValues.Length < 2)
                throw new ArgumentException();

            var rValue = ConvertType.ToDouble(rpValues[0]);
            var rParameter = ConvertType.ToDouble(rpValues[1]);

            return rValue <= rParameter;
        }

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
        public object[] ConvertBack(object rpValue, Type[] rpTargetTypes, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }
}
